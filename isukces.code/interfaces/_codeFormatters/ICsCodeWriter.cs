// ReSharper disable MemberCanBePrivate.Global

using System;
using System.Collections.Generic;
using System.Linq;

namespace iSukces.Code.Interfaces;

public interface ICsCodeWriter : ICodeWriter
{
}

public static class CsCodeWriterExtensions
{
    public static void AddNamespaces(this ICsCodeWriter src, string namespaceName)
    {
        src.WriteLine("using {0};", namespaceName);
    }

    public static void AddNamespaces(this ICsCodeWriter src, IEnumerable<string> namespaceName)
    {
        foreach (var x in namespaceName)
            src.AddNamespaces(x);
    }


    public static void CloseCompilerIf(this ICsCodeWriter self, bool close)
    {
        if (close)
            self.WritelineNoIndent("#endif");
    }

    public static void CloseCompilerIf(this ICsCodeWriter self, string? directive)
    {
        if (!string.IsNullOrEmpty(directive))
            self.WritelineNoIndent("#endif");
    }


    public static void CloseCompilerIf(this ICsCodeWriter self, IConditional? conditional)
    {
        self.CloseCompilerIf(conditional?.CompilerDirective);
    }

    public static void ClosePragmaWarnings(this ICsCodeWriter self, IList<CsPragmaWarning> list)
    {
        PragmaWarnings(self, list, false);
    }

    public static T EmptyLine<T>(this T self, bool skip = false)
        where T : ICodeWriter
    {
        if (!skip)
            self.Append("\r\n");
        return self;
    }

    public static string GetIndent(this ICodeWriter self)
    {
        return self.Indent > 0 ? new string(' ', self.Indent * 4) : "";
    }


    public static bool OpenCompilerIf(this ICsCodeWriter self, string? directive)
    {
        if (string.IsNullOrEmpty(directive)) return false;
        self.WritelineNoIndent(CompilerDirectives.If + " " + directive);
        return true;
    }

    public static bool OpenCompilerIf(this ICsCodeWriter self, IConditional? conditional)
    {
        return self.OpenCompilerIf(conditional?.CompilerDirective);
    }


    public static ICodeWriter OpenConstructor(this ICodeWriter self, string x, string? baseOrThis = null)
    {
        self.WriteLine(x);
        if (!string.IsNullOrEmpty(baseOrThis))
        {
            self.Indent++;
            self.WriteLine(": " + baseOrThis);
            self.Indent--;
        }

        self.WriteLine("{");
        self.Indent++;
        return self;
    }

    public static T OpenIf<T>(this T src, string condition)
        where T : ICsCodeWriter
    {
        src.Open($"if ({condition})");
        return src;
    }

    public static void OpenPragmaWarnings(this ICsCodeWriter self, IList<CsPragmaWarning> list)
    {
        PragmaWarnings(self, list, true);
    }

    public static T OpenSwitch<T>(this T src, string expression)
        where T : ICsCodeWriter
    {
        src.Open($"switch ({expression})");
        return src;
    }

    private static void PragmaWarnings(ICsCodeWriter self, IList<CsPragmaWarning>? list, bool start)
    {
        if (list is null || list.Count == 0)
            return;
        foreach (var gr in GetActions()
                     .GroupBy(a => a.Item1)
                     .OrderBy(a => a.Key))
        {
            var items = gr.Select(b => b.Item2)
                .Distinct()
                .OrderBy(u => u)
                .ToArray();

            var list1 = string.Join(", ", items);
            self.WritelineNoIndent($"#pragma warning {gr.Key} {list1}");
        }

        return;

        IEnumerable<(string, string)> GetActions()
        {
            foreach (var el in list)
            {
                var name = el.Name?.Trim();
                if (string.IsNullOrEmpty(name))
                    continue;
                var action = el.Enabling == Enabling.Enable ? "enable" : "disable";
                if (!start)
                    action = "restore";
                yield return (action, name);
            }
        }
    }

    public static T SingleLineIf<T>(this T src, string condition, string statement, string? elseStatement = null)
        where T : ICsCodeWriter
    {
        src.WriteLine("if (" + condition + ")");
        src.IncIndent();
        src.WriteLine(statement);
        src.DecIndent();
        if (string.IsNullOrEmpty(elseStatement))
            return src;
        src.WriteLine("else");
        src.IncIndent();
        src.WriteLine(elseStatement);
        src.DecIndent();
        return src;
    }

    public static T SingleLineIfThrow<T>(this T src, string condition, CsType type, string? arg = null)
        where T : ICsCodeWriter
    {
        var statement = type.ThrowNew(arg);
        return src.SingleLineIf(condition, statement + ";");
    }

    public static void WriteComment(this ICsCodeWriter writer, ICommentable? c)
    {
        var comment = c?.GetComments()?.Trim();
        if (string.IsNullOrEmpty(comment))
            return;
        var lines = comment.Replace("\r\n", "\n").Split('\n');
        if (lines.Length == 1)
        {
            writer.WriteLine("// " + lines[0]);
        }
        else
        {
            writer.WriteLine("/*");
            foreach (var line in lines)
                writer.WriteLine(line);
            writer.WriteLine("*/");
        }
    }
    
    public static T WriteIndent<T>(this T self)
        where T : ICodeWriter
    {
        if (self.Indent > 0)
            self.Append(new string(' ', self.Indent * CodeFormatting.IndentSpaces));
        return self;
    }

    public static void WriteLambda(this ICsCodeWriter writer, string header, string expression, int maxLineLength,
        bool addSemiColon)
    {
        var emptyHeader = string.IsNullOrEmpty(header);
        if (emptyHeader)
        {
            writer.WriteLine($"=> {expression};");
            return;
        }

        if (addSemiColon)
            expression += ";";

        header = $"{header} => ";
        var lineLength = header.Length + expression.Length + writer.Indent * CodeFormatting.IndentSpaces;
        if (lineLength <= maxLineLength)
            writer.WriteLine(header + expression);
        else
            writer.WriteLine(header).IncIndent().WriteLine(expression).DecIndent();
    }
    
    public static void WriteLambda(this ICsCodeWriter writer, string header, IReadOnlyList<string> expression,
        int maxLineLength)
    {
        switch (expression.Count)
        {
            case 0:
                throw new InvalidOperationException("No expression lines");
            case 1:
                WriteLambda(writer, header, expression.Single(), maxLineLength, true);
                return;
        }

        WriteLambda(writer, header, expression[0], maxLineLength, false);
        writer.IncIndent();
        for (var index = 1; index < expression.Count; index++)
        {
            var line = expression[index];
            if (index == expression.Count - 1)
                line += ";";
            writer.WriteLine(line);
        }

        writer.DecIndent();
    }

    public static T WriteMultiLineSummary<T>(this T src, IReadOnlyList<string>? lines, bool skipIfEmpty = false)
        where T : ICsCodeWriter

    {
        lines ??= [];
        if (lines.Count == 0 && skipIfEmpty)
            return src;
        src.WriteLine("/// <summary>");
        foreach (var line in lines)
            src.WriteSummary(line);
        src.WriteLine("/// </summary>");
        return src;
    }

    public static T WriteNewLineAndIndent<T>(this T self)
        where T : ICodeWriter
    {
        self.WriteLine();
        if (self.Indent > 0)
            self.Append(new string(' ', self.Indent * CodeFormatting.IndentSpaces));
        return self;
    }

    public static T WriteSingleLineSummary<T>(this T src, string? x, bool skipIfEmpty = false)
        where T : ICsCodeWriter

    {
        if (x is null)
        {
            if (skipIfEmpty)
                return src;
            return WriteMultiLineSummary(src, XArray.Empty<string>());
        }

        var lines = x.Split('\r', '\n').Where(q => !string.IsNullOrEmpty(q?.Trim()))
            .ToArray();
        return WriteMultiLineSummary(src, lines, skipIfEmpty);
    }


    public static T WriteSummary<T>(this T src, string x)
        where T : ICsCodeWriter
    {
        // System.Xml.Linq.XObject el = new XElement("param", new XAttribute("name", p.Name), p.Description);
        // cs.Writeln("/// {0}", el);
        src.WriteLine("/// {0}", x);
        return src;
    }
}
