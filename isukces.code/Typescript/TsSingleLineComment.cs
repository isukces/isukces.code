using iSukces.Code.Interfaces;

namespace iSukces.Code.Typescript;

public class TsSingleLineComment : ITsCodeProvider
{
    public TsSingleLineComment(string? text = null)
    {
        Text = text;
    }

    public void WriteCodeTo(ITsCodeWriter writer)
    {
        if (string.IsNullOrWhiteSpace(Text))
            return;
        var lines = Text.SplitToLines();
        foreach (var line in lines)
            writer.WriteLine("// " + line);
    }

    public string Text { get; set; }
}