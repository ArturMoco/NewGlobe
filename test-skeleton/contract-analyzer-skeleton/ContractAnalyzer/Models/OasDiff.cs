using System.Text.Json.Serialization;

namespace ContractAnalyzer.Models;

public class OasDiff
{
    [JsonPropertyName("breakingChanges")]
    public List<Change>? BreakingChanges { get; set; }

    [JsonPropertyName("nonBreakingChanges")]
    public List<Change>? NonBreakingChanges { get; set; }

    [JsonPropertyName("summary")]
    public Summary? Summary { get; set; }
}

public class Change
{
    public string? Path { get; set; }
    public string? Method { get; set; }
    public string? Change { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }
    public string? Impact { get; set; }
    public string? Param { get; set; }
    public string? Property { get; set; }
    public string? Note { get; set; }
    public string? Why { get; set; }
}

public class Summary
{
    public int BreakingCount { get; set; }
    public int NonBreakingCount { get; set; }
    public string? Risk { get; set; }
}
