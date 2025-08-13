# Test Plan — Contract Analyzer (CDC)

## Scope
Validate the .NET Contract Analyzer skeleton end-to-end:
- **Input**: `oasdiff_output.json` produced from two OpenAPI files.
- **Processing**: classification + Markdown report generation.
- **Output**: `contract_report.md` and exit code signaling (for CI gating).

## Test Types
### Unit
- Parse OasDiff JSON (happy path + missing fields).
- Risk classification heuristic (`type-changed` => High).
- Markdown table formatting.

### Integration
- Run end-to-end with a sample diff and assert non-zero exit code on High.

### Contract (CDC)
- Ensure the pipeline runs `oasdiff` and stores JSON at expected path.

### Negative
- Missing `oasdiff_output.json` should exit with code 2.

### Non-functional
- Fast execution (< 2s typical), deterministic output.

## Environments
- Local dev (Windows/Linux/macOS).
- CI (GitHub Actions / Jenkins).

## Data & Seeds
- Use `/samples/oasdiff_output.json` as baseline.
- For regression, keep minimal stable samples under `samples/`.

## Acceptance Criteria
- `contract_report.md` contains Executive Summary, Breaking list, Mitigation, Test Suggestions.
- Exit code = 1 when at least one High breaking is present (no mitigation auto-approval here).
- Exit code = 0 when no breaking or only non-breaking changes.
