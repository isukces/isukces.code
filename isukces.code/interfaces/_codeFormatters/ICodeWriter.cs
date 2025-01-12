#define _AutoCloseText
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace iSukces.Code.Interfaces;

public interface ICodeWriter
{
    void Append(string text);

    string Code { get; }

    ILangInfo LangInfo { get; }

    int Indent { get; set; }
}

public static class CodeFormatterExt
{
    public static T Close<T>(this T writer, string customCloseText)
        where T : ICodeWriter
    {
        writer.DecIndent();
        writer.WriteLine(customCloseText);
        return writer;
    }

    public static T Close<T>(this T writer, bool addEmptyLine = false, string? appendText= null)
        where T : ICodeWriter
    {
        var langInfo = writer.LangInfo;
        writer.DecIndent();
#if AutoCloseText
            if (AutoCloseComment)
                src.WriteLine(langInfo.CloseText + " " + langInfo.OneLineComment + " " + opening[Indent]);
            else
#endif
        if (!string.IsNullOrEmpty(appendText))
            writer.WriteLine(langInfo.CloseText + " " + appendText.Trim());
        else
            writer.WriteLine(langInfo.CloseText);
        if (addEmptyLine)
            writer.WriteLine();
        return writer;
    }

    public static T CloseNl<T>(this T writer)
        where T : ICodeWriter
    {
        writer.Close();
        writer.WriteLine(string.Empty);
        return writer;
    }

    public static T DecIndent<T>(this T writer)
        where T : ICodeWriter
    {
        writer.Indent--;
        return writer;
    }

    public static T DoWithKeepingIndent<T>(this T writer, Action action)
        where T : ICodeWriter
    {
        var indentBefore = writer.Indent;
        action();
        if (writer.Indent == indentBefore) return writer;
        // some warning should be created here
        writer.Indent = indentBefore;
        return writer;
    }

    public static string GetCodeTrim(this ICodeWriter writer)
    {
        if (writer == null) throw new ArgumentNullException(nameof(writer));
        return writer.Code.Trim();
    }

    public static T IncIndent<T>(this T writer)
        where T : ICodeWriter
    {
        writer.Indent++;
        return writer;
    }


    public static T Open<T>(this T writer, string x)
        where T : ICodeWriter
    {
#if AutoCloseText
            opening[src.Indent] = x;
#endif
        writer.WriteLine(x);
        writer.WriteLine(writer.LangInfo.OpenText);
        writer.Indent++;
        return writer;
    }

    public static T Open<T>(this T writer, string format, params object[] args)
        where T : ICodeWriter
    {
        writer.Open(string.Format(format, args));
        return writer;
    }

    public static T OpenBrackets<T>(this T writer)
        where T : ICodeWriter
    {
        return writer.WriteLine(writer.LangInfo.OpenText).IncIndent();
    }

    public static T SplitWriteLine<T>(this T writer, string? text)
        where T : ICodeWriter
    {
        if (string.IsNullOrEmpty(text))
            return writer;
        var query = from i in text!.Split('\r', '\n')
            where !string.IsNullOrEmpty(i)
            select i.TrimEnd();
        foreach (var i in query)
            writer.WriteLine(i);
        return writer;
    }

    // [Obsolete("Use CsType instead of string", GlobalSettings.WarnObsolete)]
    public static T WithOpen<T>(this T writer, string text)
        where T : ICodeWriter
    {
        writer.Open(text);
        return writer;
    }

    /*
    public static T WithClose<T>(this T writer)
        where T : ICodeWriter
    {
        writer.Close();
        return writer;
    }*/

    public static T WriteLine<T>(this T writer)
        where T : ICodeWriter
    {
        writer.Append("\r\n");
        return writer;
    }

    public static T WriteLine<T>(this T writer, string text)
        where T : ICodeWriter
    {
        writer.WriteIndent().Append(text + "\r\n");
        return writer;
    }
    
    [StringFormatMethod("format")]
    public static T WriteLine<T>(this T writer, string format, params object[] args)
        where T : ICodeWriter
    {
        var text = string.Format(format, args);
        return writer.WriteLine(text);
    }

    public static T WritelineNoIndent<T>(this T writer, string compilerCode)
        where T : ICodeWriter
    {
        var indentBefore = writer.Indent;
        writer.Indent = 0;
        writer.WriteLine(compilerCode);
        writer.Indent = indentBefore;
        return writer;
    }

    public static T WriteLines<T>(this T writer, IEnumerable<string> texts)
        where T : ICodeWriter
    {
        foreach (var line in texts)
            WriteLine(writer, line);
        return writer;
    }
}
