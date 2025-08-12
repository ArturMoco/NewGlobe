# Example — Contract Analyzer in the Contract Layer (CDC)

## 1. Strategy Objective
This strategy ensures:

- **Safety and predictability** in deliveries.  
- **Prevention** of regressions and breaking changes for API consumers.  
- **Agility** through the use of an **AI QA Agent** specialized in contract analysis.  
- **Clear communication** with the entire team, including those who are not QA specialists.  

---

## 2. Test Pyramid — Focus on the Contract Layer (CDC)

| Layer           | Objective                                    | Tools                                                                                   | AI Agent (Role)         | Agent Output                                                                                                                                             | Approval Criteria                                      |
| --------------- | -------------------------------------------- | --------------------------------------------------------------------------------------- | ----------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------ |
| **Contract (CDC)** | Prevent breaking changes for consumers       | `oasdiff` (compares versions of the **OpenAPI** specification, which defines API endpoints, parameters, and responses) | **Contract Analyzer**   | Markdown report with:<br>• Executive summary<br>• Risk matrix<br>• Remediation plan<br>• Additional test suggestions                                    | No breaking changes without a mitigation plan         |

---

## 3. How the AI QA Agent Works

Before detailing the steps, it's important to understand that **oasdiff** and **GPT** serve different, complementary purposes:

- **`oasdiff`** is the *objective sensor* — it performs precise, deterministic comparisons between two OpenAPI specifications, detecting any contract changes without ambiguity.
- **GPT** is the *junior analyst* — it reads the diff results, classifies risks, and suggests mitigation and tests in a human-readable way.

> 💡 **Summary for interview**: “`oasdiff` is my objective sensor, detecting every contract change. GPT is my junior analyst, classifying risks and suggesting actions. One does not replace the other — they complement each other.”


The **Contract Analyzer** is a utility built in **.NET** that:

1. Runs `oasdiff` between the old and new **OpenAPI** specification (*OpenAPI Specification*, or “API contract”).
2. Analyzes the result and identifies critical changes in the contract.
3. Uses GPT (trained only with public documentation of OpenAPI, Swagger, and contract testing best practices) to:
   - Classify the risk of each change.
   - Suggest remediation (*fix* or *version bump*).
   - Propose test ideas to cover the change.
4. Generates a **Markdown** report attached to the Pull Request (PR).
5. **Fails the build** if there are breaking changes without mitigation.

> 💡 **Note:** The AI Agent acts as a “Junior QA” for repetitive tasks.  
> Final review and strategic decisions remain with the **QA Lead**.

---

## 4. Visual Example — OpenAPI Before/After and Diff

### 4.1 Before (Version 1.0.0)
```yaml
openapi: 3.0.3
info:
  title: Feature Toggle API
  version: 1.0.0
paths:
  /v1/features/{key}:
    get:
      summary: Get feature by key
      parameters:
        - in: path
          name: key
          required: true
          schema: { type: string }
      responses:
        '200':
          description: OK
          content:
            application/json:
              schema:
                type: object
                required: [ key, value ]
                properties:
                  key: { type: string }
                  value: { type: boolean }
```

### 4.2 After (Version 1.1.0)
```yaml
openapi: 3.0.3
info:
  title: Feature Toggle API
  version: 1.1.0
paths:
  /v1/features/{key}:
    get:
      summary: Get feature by key
      parameters:
        - in: path
          name: key
          required: true
          schema: { type: string }
      responses:
        '200':
          description: OK
          content:
            application/json:
              schema:
                type: object
                required: [ key, value ]
                properties:
                  key: { type: string }
                  value:
                    type: object
                    required: [ status ]
                    properties:
                      status: { type: boolean }
                      source: { type: string, enum: [default, variation] }
```

---

### 4.3 Diff Detected by `oasdiff`
```json
{
  "breakingChanges": [
    {
      "path": "/v1/features/{key}",
      "method": "get",
      "change": "type-changed",
      "from": "value:boolean",
      "to": "value:object(status:boolean,source:string)",
      "impact": "High",
      "why": "Consumers expect boolean; object will break deserialization."
    }
  ],
  "nonBreakingChanges": [
    {
      "path": "/v1/features/{key}",
      "method": "get",
      "change": "added-property",
      "property": "value.source",
      "note": "New optional property."
    }
  ],
  "summary": { "breakingCount": 1, "nonBreakingCount": 1, "risk": "High" }
}
```

---

## 5. Example PR Output (Agent Report)

```markdown
# Contract Report – Feature Toggle API

## OpenAPI Comparison (oasdiff)
- Breaking: 1
- Non-breaking: 1

## AI Agent Analysis
**Executive Summary:**
• One high-impact contract change.  
• Recommendation: create new `/v2` version for the new format.

**Risk Matrix:**
1. Change in `value` type — **High** — Affects all consumers expecting boolean.

**Remediation Plan:**
• Keep `/v1` with `value:boolean`.  
• Create `/v2` with new object `{ status, source }`.

**Test Suggestions:**
• Pact contract for `/v1/features/{key}` validating boolean.  
• Pact contract for `/v2/features/{key}` validating object.
```

---

## 6. Pipeline Integration

- **PR Validation:** Runs the **Contract Analyzer** and publishes report.  
- **Gate:** If there is a breaking change without mitigation, the build fails.  
- **Artifacts:** `.md` report stored and attached to the PR.  
- **Slack/Teams Integration:** Notification in case of failure.

---

## 7. Best Practices

- Maintain API versioning (`/v1`, `/v2`) to avoid immediate breaking changes.  
- Run the Contract Analyzer on every `openapi.yaml` change.  
- Periodically review contracts with other consumer teams.  
- Avoid overload with **unnecessary E2E** — focus on real impact.

---

## 8. Benefits

- Drastically reduces contract review time.  
- Detects risks before merge.  
- Standardizes analysis and technical documentation.  
- Frees the QA Lead to focus on higher-impact decisions.  

---

📌 **Author:** Artur Felipe Albuquerque Portela – QA Engineer / SDET  
