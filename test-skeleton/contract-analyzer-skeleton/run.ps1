# Run analyzer locally on Windows PowerShell
# 1) Ensure oasdiff is installed and two OpenAPI files exist.
# 2) Generate diff JSON and run the .NET console app.

oasdiff breaking .\openapi_old.yaml .\openapi_new.yaml --format json > .\ContractAnalyzer\oasdiff_output.json
dotnet build .\ContractAnalyzer\ContractAnalyzer.csproj -c Release
dotnet run --project .\ContractAnalyzer\
