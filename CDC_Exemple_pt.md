# Exemplo — Contract Analyzer na Camada de Contratos (CDC)

## 1. Objetivo da Estratégia
Esta estratégia garante:

- **Segurança e previsibilidade** nas entregas.  
- **Prevenção** de regressões e *breaking changes* para consumidores da API.  
- **Agilidade** com uso de um **Agente QA IA** especializado em análise de contratos.  
- **Comunicação clara** com toda a equipe, inclusive para quem não é especialista em QA.  

---

## 2. Pirâmide de Testes — Foco na Camada de Contratos (CDC)

| Camada          | Objetivo                                     | Ferramentas                                                                 | Agente IA (Função)      | Saída do Agente                                                                                                                                          | Critério de Aprovação                                   |
| --------------- | -------------------------------------------- | --------------------------------------------------------------------------- | ----------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------- |
| **Contrato (CDC)** | Evitar *breaking changes* para consumidores | `oasdiff` (compara versões da especificação **OpenAPI**, que define endpoints, parâmetros e respostas de uma API) | **Contract Analyzer**   | Relatório em Markdown com:<br>• Resumo executivo<br>• Matriz de risco<br>• Plano de remediação<br>• Sugestões de testes adicionais                      | Nenhuma *breaking change* sem plano de mitigação        |

---

## 3. Como o Agente QA IA Opera

O **Contract Analyzer** é um utilitário em **.NET** que:

1. Executa o `oasdiff` entre a versão antiga e a nova da especificação **OpenAPI** (*OpenAPI Specification*, ou “contrato” da API).
2. Analisa o resultado e identifica mudanças críticas no contrato.
3. Usa GPT (treinado apenas com documentação pública de OpenAPI, Swagger e práticas de *contract testing*) para:
   - Classificar o risco de cada mudança.
   - Sugerir remediação (*fix* ou *version bump*).
   - Propor ideias de testes para cobrir a alteração.
4. Gera um relatório em **Markdown** anexado ao Pull Request (PR).
5. **Falha o build** caso haja *breaking changes* sem mitigação.

> 💡 **Nota:** O Agente IA atua como “QA Júnior” para tarefas repetitivas.  
> A revisão final e decisão estratégica continuam sendo do **QA Lead**.

---

## 4. Exemplo Visual — OpenAPI Antes/Depois e Diff

### 4.1 Antes (Versão 1.0.0)
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

### 4.2 Depois (Versão 1.1.0)
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

### 4.3 Diff Detectado pelo `oasdiff`
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
      "why": "Consumidores esperam boolean; objeto quebra a desserialização."
    }
  ],
  "nonBreakingChanges": [
    {
      "path": "/v1/features/{key}",
      "method": "get",
      "change": "added-property",
      "property": "value.source",
      "note": "Nova propriedade opcional."
    }
  ],
  "summary": { "breakingCount": 1, "nonBreakingCount": 1, "risk": "High" }
}
```

---

## 5. Exemplo de Saída no PR (Relatório do Agente)

```markdown
# Contract Report – Feature Toggle API

## Comparação OpenAPI (oasdiff)
- Breaking: 1
- Non-breaking: 1

## Análise do Agente IA
**Resumo Executivo:**
• Uma alteração de contrato com impacto alto.  
• Recomendação: criar nova versão `/v2` para o novo formato.

**Matriz de Risco:**
1. Alteração no tipo de `value` — **Alto** — Afeta todos os consumidores que esperam boolean.

**Plano de Remediação:**
• Manter `/v1` com `value:boolean`.  
• Criar `/v2` com novo objeto `{ status, source }`.

**Sugestões de Teste:**
• Contrato Pact para `/v1/features/{key}` validando boolean.  
• Contrato Pact para `/v2/features/{key}` validando objeto.
```

---

## 6. Integração no Pipeline

- **PR Validation:** Executa o **Contract Analyzer** e publica relatório.  
- **Gate:** Se houver *breaking change* sem mitigação, o build falha.  
- **Artefatos:** Relatório `.md` armazenado e anexado no PR.  
- **Integração com Slack/Teams:** Notificação em caso de falha.

---

## 7. Boas Práticas

- Manter o versionamento de API (`/v1`, `/v2`) para evitar quebra imediata.  
- Executar o Contract Analyzer em todas as alterações de `openapi.yaml`.  
- Revisar periodicamente contratos com outros times consumidores.  
- Evitar sobrecarga com **E2E desnecessário** — foco no impacto real.

---

## 8. Benefícios

- Reduz drasticamente o tempo de revisão de contratos.  
- Detecta riscos antes do *merge*.  
- Padroniza a análise e a documentação técnica.  
- Libera o QA Lead para focar em decisões de maior impacto.  

---

📌 **Autor:** Artur Felipe Albuquerque Portela – QA Engineer / SDET  
