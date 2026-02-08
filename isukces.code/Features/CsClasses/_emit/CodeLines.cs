using System;
using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Interfaces;

namespace iSukces.Code;

public class CodeLines
{
    public CodeLines(IReadOnlyList<string>? lines, bool isExpressionBody = false)
        : this(lines, isExpressionBody, true)
    {
    }


    protected CodeLines(IReadOnlyList<string>? lines, bool isExpressionBody, bool filterEmptyLines)
    {
        if (filterEmptyLines)
        {
            Lines = FilterEmpty(lines);
        }
        else
        {
            Lines = lines ?? XArray.Empty<string>();
        }

        IsExpressionBody = isExpressionBody;
    }

    private static string[] FilterEmpty(IReadOnlyList<string>? lines)
    {
        if (lines is null || lines.Count == 0)
            return XArray.Empty<string>();
        return lines
            .Where(a => !string.IsNullOrWhiteSpace(a))
            .ToArray();
    }

    public static CodeLines FromExpression(string expression)
    {
        return new CodeLines([expression], true, false);
    }

    public static CodeLines Parse(string? body, bool methodIsExpressionBody)
    {
        if (string.IsNullOrWhiteSpace(body))
            return new CodeLines(XArray.Empty<string>(), methodIsExpressionBody, false);
        return new CodeLines(body.Split('\r', '\n'), methodIsExpressionBody);
    }

    public CodeLines AddSemicolon()
    {
        if (IsEmpty)
            return this;
        var a = Lines.ToArray();
        a[0] = a[0].TrimEnd(';').TrimEnd() + ";";
        return new CodeLines(a, IsExpressionBody, false);
    }

    public string GetExpression()
    {
        if (IsEmpty)
            throw new InvalidOperationException("No code here");
        var a = string.Join("\n", Lines);
        a = a.Trim().TrimEnd(';', ' ', '\t');
        return a;
    }

    public IReadOnlyList<string> GetExpressionLines(string? firstLinePrefix, bool addSemiColon)
    {
        var e = GetExpression().SplitToLines();
        if (!string.IsNullOrEmpty(firstLinePrefix))
            e[0] = firstLinePrefix + " " + e[0];
        if (addSemiColon)
            e[^1] += ";";
        return e;
    }

    public override string ToString()
    {
        if (IsExpressionBody)
            return "=>" + string.Join("\r\n", Lines);
        return string.Join("\r\n", Lines);
    }

    public void WriteExpressionLines(string firstLinePrefix, bool addSemiColon, ICsCodeWriter csCodeWriter)
    {
        var lines = GetExpressionLines(firstLinePrefix, addSemiColon);
        for (var index = 0; index < lines.Count; index++)
        {
            csCodeWriter.WriteLine(lines[index]);
            if (index == 0)
            {
                if (lines.Count == 1)
                    return;
                csCodeWriter.Indent++;
            }

            csCodeWriter.Indent--;
        }
    }

    public void WriteLines(ICsCodeWriter writer, bool indent)
    {
        if (indent)
            writer.Indent++;
        foreach (var iii in Lines)
            writer.WriteLine(iii);
        if (indent)
            writer.Indent--;
    }

    public bool IsEmpty => Lines.Count == 0;

    public IReadOnlyList<string> Lines { get; }

    public bool IsExpressionBody { get; set; }
    public int  LinesCount       => Lines.Count;
}

