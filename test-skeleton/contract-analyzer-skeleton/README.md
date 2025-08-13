# Contract Analyzer (CDC) — .NET Skeleton

This repository is a **skeleton** for a .NET 8 console application that analyzes **OpenAPI contract changes** using `oasdiff`, generates a **Markdown report**, and **fails the CI** when there are **high-risk breaking changes without mitigation**.

## Highlights
- ✅ .NET 8 console app
- ✅ Reads `oasdiff` JSON and builds a report
- ✅ Pluggable GPT client via `IGptClient` (uses a mock by default)
- ✅ Works on Windows (PowerShell) and Linux/macOS (bash)
- ✅ CI examples for **GitHub Actions** and **Jenkins**

## Quickstart (Windows PowerShell)
```powershell
# 1) Put your OpenAPI files at the repository root
#    - openapi_old.yaml
#    - openapi_new.yaml

# 2) Run the helper script
./run.ps1

# Output:
# - ContractAnalyzer/oasdiff_output.json
# - ContractAnalyzer/contract_report.md
# - Non-zero exit code if high-risk breaking changes are found without mitigation
```

## Project Structure
```
ContractAnalyzer/
  ContractAnalyzer.csproj
  Program.cs
  Models/
    OasDiff.cs
  Services/
    ContractReportService.cs
    Gpt/
      IGptClient.cs
      MockGptClient.cs
  Utils/
    MarkdownBuilder.cs
ci/
  Jenkinsfile
.github/workflows/contract-analyzer.yml
docs/
samples/
```

## How to plug a real GPT/LLM client
Implement `IGptClient` (in `Services/Gpt`) and replace `MockGptClient` in `Program.cs`. Keep secrets in CI variables.

## CI Gates (recommendation)
- ❌ Fail PR if: high-risk breaking changes without a mitigation plan.
- ✅ Pass PR if: no breaking changes or all mitigations documented.

## License
MIT (adjust as needed).
