namespace ContractAnalyzer.Services.Gpt;

public interface IGptClient
{
    string Summarize(string prompt);
}
