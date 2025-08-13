using ContractAnalyzer.Services;
using ContractAnalyzer.Services.Gpt;

const string JsonPath = "oasdiff_output.json";
const string ReportPath = "contract_report.md";

var reportService = new ContractReportService(new MockGptClient());

if (!File.Exists(JsonPath))
{
    Console.Error.WriteLine($"❌ File not found: {JsonPath}");
    Environment.Exit(2);
}

var (markdown, hasHighWithoutMitigation) = reportService.BuildReport(JsonPath);
File.WriteAllText(ReportPath, markdown);
Console.WriteLine($"📄 Report generated: {ReportPath}");

if (hasHighWithoutMitigation)
{
    Console.Error.WriteLine("🚨 High-risk breaking change detected. Require mitigation before proceeding.");
    Environment.Exit(1); // Fail pipeline
}
