#!/usr/bin/env bash
# Run analyzer locally on macOS/Linux
set -euo pipefail
oasdiff breaking ./openapi_old.yaml ./openapi_new.yaml --format json > ./ContractAnalyzer/oasdiff_output.json
dotnet build ./ContractAnalyzer/ContractAnalyzer.csproj -c Release
dotnet run --project ./ContractAnalyzer/
