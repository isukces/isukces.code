#nullable enable
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

        var flag = EndingStyle == FluentCodeEndingStyle.EndingAfterDecIndent;
        var lastIdx = CodeLines.Count - 1;
        var isIndent = false;
        for (var index = 0; index < CodeLines.Count; index++)
        {
            var line = CodeLines[index];
            if (index == 0)
            {
                if (flag)
                {
                    writer.WriteLine(FirstLineStart);
                    X();
                }
                else
                {
                    line = FirstLineStart + line;
                }
            }
            else
                line = NextLineStart + line;

            if (index == lastIdx)
            {
                if (!flag)
                    line += Ending;
            }
            else
            {
                line += Separator;
            }

            writer.WriteLine(line);
            if (index == 0)
                X();
        }

        if (isIndent)
            writer.DecIndent();
        if (flag)
            writer.WriteLine(Ending);
        return;

        void X()
        {
            if (!isIndent)
            {
                isIndent = true;
                writer.IncIndent();
            }
        }
    }

    public int Count => CodeLines.Count;
    public string FirstLineStart { get; set; }
    public string NextLineStart { get; set; } = ".";
    public string Ending { get; set; } = ";";
    public string EmptyCode { get; set; }
    public string Separator { get; set; }
    public List<string> CodeLines { get; } = [];
    public FluentCodeEndingStyle EndingStyle { get; set; }

    private readonly List<string> _comments = [];

    public FluentCode SetCreateObject(string variable, string constructor, bool addSemicolon, bool x)
    {
        FirstLineStart = $"{variable} = {constructor} {{";
        Ending = "}";
        EmptyCode = $"{variable} = {constructor}";
        NextLineStart = "";
        Separator = ",";
        EndingStyle = FluentCodeEndingStyle.EndingAfterDecIndent;
        if (x)
            EmptyCode += "()";
        if (addSemicolon)
        {
            Ending += ";";
            EmptyCode += ";";
        }

        return this;

    }
}

public enum FluentCodeEndingStyle
{
    Default,
    EndingAfterDecIndent
}