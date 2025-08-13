using System.Text;

namespace ContractAnalyzer.Utils;

public class MarkdownBuilder
{
    private readonly StringBuilder _sb = new();

    public MarkdownBuilder H1(string text) { _sb.AppendLine($"# {text}").AppendLine(); return this; }
    public MarkdownBuilder H2(string text) { _sb.AppendLine($"
## {text}
"); return this; }
    public MarkdownBuilder Line(string text = "") { _sb.AppendLine(text); return this; }
    public MarkdownBuilder TableHeader(params string[] headers)
    {
        _sb.AppendLine("| " + string.Join(" | ", headers) + " |");
        _sb.AppendLine("|" + string.Join("|", headers.Select(_ => "---")) + "|");
        return this;
    }
    public MarkdownBuilder TableRow(params string[] cols)
    {
        _sb.AppendLine("| " + string.Join(" | ", cols) + " |");
        return this;
    }
    public override string ToString() => _sb.ToString();
}
