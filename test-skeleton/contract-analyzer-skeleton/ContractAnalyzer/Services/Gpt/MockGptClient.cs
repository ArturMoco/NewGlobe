namespace ContractAnalyzer.Services.Gpt;

public class MockGptClient : IGptClient
{
    public string Summarize(string prompt)
    {
        // Offline stub for demo purposes. Replace with a real client when needed.
        return "- Agent summary: high-impact contract changes require mitigation (introduce /v2 or preserve backward compatibility).";
    }
}
