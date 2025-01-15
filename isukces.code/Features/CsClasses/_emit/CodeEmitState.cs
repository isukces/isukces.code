using iSukces.Code.Interfaces;

namespace iSukces.Code;

public sealed class CodeEmitState
{
    public void Flush(ICsCodeWriter writer)
    {
        CloseDirective(writer);
        StartItem(writer, "");
    }

    private void StartItem(ICsCodeWriter writer, string directive)
    {
        if (directive != _currentDirective) 
            CloseDirective(writer);
        
        if (WriteEmptyLine)
        {
            writer.EmptyLine();
            WriteEmptyLine = false;
        }

        if (directive != _currentDirective)
        {
            writer.OpenCompilerIf(directive);
            _currentDirective = directive;
        }
    }

    private void CloseDirective(ICsCodeWriter writer)
    {
        writer.CloseCompilerIf(_currentDirective);
        _currentDirective = "";
    }

    public void StartItem(ICsCodeWriter writer, IConditional? conditional)
    {
        var expected = conditional?.CompilerDirective ?? "";
        StartItem(writer, expected);
    }

    public bool WriteEmptyLine { get; set; }

    private string _currentDirective = "";
}
