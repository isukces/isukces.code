using System.Text;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Typescript;

public class TsField : TsMethodArgument, ITsClassMember
{
    public TsField()
    {
    }

    public TsField(string name)
    {
        Name = name;
    }

    public override string ToString()
    {
        return "TsField: " + GetHeaderItems(false);
    }

    public void WriteCodeTo(ITsCodeWriter writer)
    {
        Introduction?.WriteCodeTo(writer);
        var code = GetHeaderItems(writer.HeadersOnly);
        writer.WriteLine(code);
    }

    private string GetHeaderItems(bool headerOnly)
    {
        var sb = new StringBuilder();
        if (Visibility != TsVisibility.Default)
            sb.Append(Visibility.ToString().ToLower());
        if (IsStatic)
            sb.Append(" static");
        sb.Append(" " + Name);
        if (headerOnly && IsOptional)
            sb.Append('?');
        if (!string.IsNullOrEmpty(Type))
            sb.Append(": " + Type);
        if (!string.IsNullOrEmpty(Initializer) && !headerOnly)
            sb.Append(" = " + Initializer);
        sb.Append(';');
        if (!string.IsNullOrWhiteSpace(InlineComment))
            sb.Append(" // " + InlineComment);
        return sb.ToString().Trim();
    }

    public bool         IsStatic    { get; set; }
    public TsVisibility Visibility  { get; set; }
    public string       Initializer { get; set; }
        

    public string          InlineComment { get; set; }
    public ITsCodeProvider? Introduction  { get; set; }
}