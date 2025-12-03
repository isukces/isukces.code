#define _AutoCloseText
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace iSukces.Code.Interfaces;

public interface ICodeWriter
{
    void Append(string text);

    #region Properties

    string Code { get; }

    ILangInfo LangInfo { get; }

    int Indent { get; set; }

    #endregion
}

public static class CodeFormatterExt
{
    extension<T>(T writer)
        where T : ICodeWriter
    {
        public T Close(bool addEmptyLine = false, string? appendText = null)
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

        public T Close(string customCloseText)
        {
            writer.DecIndent();
            writer.WriteLine(customCloseText);
            return writer;
        }

        public T CloseNl()
        {
            writer.Close();
            writer.WriteLine(string.Empty);
            return writer;
        }

        public T DecIndent()
        {
            writer.Indent--;
            return writer;
        }

        public T DoWithKeepingIndent(Action action)
        {
            var indentBefore = writer.Indent;
            action();
            if (writer.Indent == indentBefore) return writer;
            // some warning should be created here
            writer.Indent = indentBefore;
            return writer;
        }


        public string GetCodeTrim()
        {
            if (writer is null) throw new ArgumentNullException(nameof(writer));
            return writer.Code.Trim();
        }

        public T IncIndent()
        {
            writer.Indent++;
            return writer;
        }

        public T Open(string x)
        {
#if AutoCloseText
            opening[src.Indent] = x;
#endif
            writer.WriteLine(x);
            writer.WriteLine(writer.LangInfo.OpenText);
            writer.Indent++;
            return writer;
        }

        public T Open(string format, params object[] args)
        {
            writer.Open(string.Format(format, args));
            return writer;
        }

        public T OpenBrackets()
        {
            return writer.WriteLine(writer.LangInfo.OpenText).IncIndent();
        }

        public T SplitWriteLine(string? text)
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

        public T WithOpen(string text)
        {
            writer.Open(text);
            return writer;
        }

        public T WriteLine()
        {
            writer.Append("\r\n");
            return writer;
        }

        public T WriteLine(string text)
        {
            writer.WriteIndent().Append(text + "\r\n");
            return writer;
        }

        [StringFormatMethod("format")]
        public T WriteLine(string format, params object[] args)
        {
            var text = string.Format(format, args);
            return writer.WriteLine(text);
        }

        public T WritelineNoIndent(string compilerCode)
        {
            var indentBefore = writer.Indent;
            writer.Indent = 0;
            writer.WriteLine(compilerCode);
            writer.Indent = indentBefore;
            return writer;
        }

        public T WriteLines(IEnumerable<string> texts)
        {
            foreach (var line in texts)
                WriteLine(writer, line);
            return writer;
        }
    }
}
