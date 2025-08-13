using System.Text.Json;
using ContractAnalyzer.Models;
using ContractAnalyzer.Services.Gpt;
using ContractAnalyzer.Utils;

namespace ContractAnalyzer.Services;

public class ContractReportService
{
    private readonly IGptClient _gpt;

    public ContractReportService(IGptClient gpt) => _gpt = gpt;

    public (string markdown, bool hasHighWithoutMitigation) BuildReport(string jsonPath)
    {
        var json = File.ReadAllText(jsonPath);
        var diff = System.Text.Json.JsonSerializer.Deserialize<OasDiff>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new OasDiff();

        var breaking = diff.BreakingChanges ?? new List<Change>();
        var nonBreaking = diff.NonBreakingChanges ?? new List<Change>();

        bool HasHigh(Change c) =>
            string.Equals(c.Impact, "High", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(c.Change, "type-changed", StringComparison.OrdinalIgnoreCase);

        var highList = breaking.Where(HasHigh).ToList();
        var needsMitigation = highList.Any();

        var prompt = $"Analyze contract changes: {breaking.Count} breaking, {nonBreaking.Count} non-breaking. High: {highList.Count}. Suggest mitigation and additional tests.";
        var agentSummary = _gpt.Summarize(prompt);

        var md = new MarkdownBuilder()
            .H1("Contract Analyzer Report – Feature Toggle API")
            .H2("📊 Executive Summary")
            .Line($"Breaking: **{breaking.Count}** — Non‑breaking: **{nonBreaking.Count}**")
            .Line(agentSummary)
            .H2("🔍 Breaking Changes")
            .TableHeader("Endpoint", "Method", "Change", "Impact", "Detail");

        foreach (var c in breaking)
        {
            var endpoint = c.Path ?? "-";
            var method = c.Method ?? "-";
            var change = c.Change ?? "-";
            var impact = c.Impact ?? (HasHigh(c) ? "High" : "—");
            var detail = c.Why ?? c.Property ?? c.Param ?? "-";
            md.TableRow(endpoint, method, change, impact, detail);
        }

        md.H2("✅ Mitigation Recommendations")
          .Line("- For `type-changed` on widely consumed fields: keep `/v1` and introduce `/v2` with the new format; deprecate legacy.
- For removed parameters: make them optional during a transition period, document in the changelog and notify consumers.")
          .H2("🧪 Additional Test Suggestions")
          .Line("- Per-consumer Pact (/v1 and `/v2`).
- Authenticated integration (Basic/OAuth) for changed endpoints.
- Staging smoke after canary deploy, validating serialization/compatibility.");

        return (md.ToString(), needsMitigation);
    }
}
