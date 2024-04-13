using System.Collections.Generic;
using iSukces.Code.Interfaces;

namespace iSukces.Code;

public class FluentCode
{
    public void AddComment(string comment)
    {
        _comments.Add(comment);
    }

    public FluentCode AddLine(string code)
    {
        CodeLines.Add(code);
        return this;
    }

    public FluentCode AddMethod(string methodName, params string[] args)
    {
        var code = args.CommaJoin().Parentheses(methodName);
        CodeLines.Add(code);
        return this;
    }

    public void Write(CsCodeWriter writer)
    {
        foreach (var i in _comments)
            writer.WriteLine($"// {i}");
        if (CodeLines.Count < 1)
        {
            if (!string.IsNullOrEmpty(EmptyCode))
                writer.WriteLine(EmptyCode);
            return;
        }

        var lastIdx = CodeLines.Count - 1;
        for (var index = 0; index < CodeLines.Count; index++)
        {
            var line = CodeLines[index];
            if (index > 0)
                line = NextLineStart + line;
            else
                line = FirstLineStart + line;
            if (index == lastIdx)
                line += Ending;
            writer.WriteLine(line);
            if (index == 0)
                writer.IncIndent();
        }

        writer.DecIndent();
    }

    public int Count => CodeLines.Count;
    public string FirstLineStart { get; set; }
    public string NextLineStart { get; set; } = ".";
    public string Ending { get; set; } = ";";
    public string EmptyCode { get; set; }

    public List<string> CodeLines { get; } = [];

    private readonly List<string> _comments = [];
}