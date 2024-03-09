#define _AutoCloseText
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace iSukces.Code.Interfaces
{
    public interface ICodeWriter
    {
        void Append(string text);

        [NotNull]
        string Code { get; }

        [NotNull]
        ILangInfo LangInfo { get; }

        int Indent { get; set; }
    }

    public static class CodeFormatterExt
    {
        public static void Close(this ICodeWriter src, string customCloseText)
        {
            src.DecIndent();
            src.WriteLine(customCloseText);
        }

        public static void Close(this ICodeWriter src, bool addEmptyLine = false)
        {
            var langInfo = src.LangInfo;
            src.DecIndent();
#if AutoCloseText
            if (AutoCloseComment)
                src.WriteLine(langInfo.CloseText + " " + langInfo.OneLineComment + " " + opening[Indent]);
            else
#endif
            src.WriteLine(langInfo.CloseText);
            if (addEmptyLine)
                src.WriteLine();
        }

        [Obsolete("use just Close method", GlobalSettings.WarnObsolete)]
        public static void CloseBrackets(this ICodeWriter src)
        {
            src.DecIndent();
            src.WriteLine(src.LangInfo.CloseText);
        }

        public static void CloseNl(this ICodeWriter src)
        {
            src.Close(false);
            src.WriteLine(string.Empty);
        }




        public static void DoWithKeepingIndent(this ICodeWriter obj, Action action)
        {
            var indentBefore = obj.Indent;
            action();
            if (obj.Indent == indentBefore) return;
            // some warning should be created here
            obj.Indent = indentBefore;
        }

        [NotNull]
        public static string GetCodeTrim([NotNull] this ICodeWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            return writer.Code.Trim();
        }


        public static T Open<T>(this T src, string x) where T:ICodeWriter
        {
#if AutoCloseText
            opening[src.Indent] = x;
#endif
            // Writeln(x + " " + LangInfo.OpenText);
            src.WriteLine(x);
            src.WriteLine(src.LangInfo.OpenText);
            src.Indent++;
            return src;
        }

        public static T Open<T>(this T src, string format, params object[] args) where T : ICodeWriter
        {
            src.Open(string.Format(format, args));
            return src;
        }


        public static void OpenBrackets(this ICodeWriter src)
        {
            src.WriteLine(src.LangInfo.OpenText);
            src.IncIndent();
        }

        public static T DecIndent<T>(this T obj)
            where T : ICodeWriter
        {
            obj.Indent--;
            return obj;
        }

        public static T IncIndent<T>(this T obj)
            where T : ICodeWriter
        {
            obj.Indent++;
            return obj;
        }

        [Obsolete("Use CsType instead of string", GlobalSettings.WarnObsolete)]
        public static T WithOpen<T>(this T obj, string text)
            where T : ICodeWriter
        {
            obj.Open(text);
            return obj;
        }

        public static T WithClose<T>(this T obj)
            where T : ICodeWriter
        {
            obj.Close();
            return obj;
        }

        public static T WriteLine<T>(this T obj)
            where T : ICodeWriter
        {
            obj.Append("\r\n");
            return obj;
        }

        public static T WriteLine<T>(this T obj, string text)
            where T : ICodeWriter
        {
            obj.WriteIndent().Append(text + "\r\n");
            return obj;
        }
        
        public static T WriteLines<T>(this T obj, IEnumerable<string> texts)
            where T : ICodeWriter
        {
            foreach (var line in texts)
                WriteLine(obj, line);
            return obj;
        }

        public static T SplitWriteLine<T>(this T x, string text)
            where T : ICodeWriter
        {
            if (string.IsNullOrEmpty(text))
                return x;
            var query = from i in text.Split('\r', '\n')
                where !string.IsNullOrEmpty(i)
                select i.TrimEnd();
            foreach (var i in query)
                x.WriteLine(i);
            return x;
        }


        [StringFormatMethod("format")]
        public static T WriteLine<T>(this T src, string format, params object[] args)
            where T : ICodeWriter
        {
            var text = string.Format(format, args);
            return src.WriteLine(text);
        }

        public static T WritelineNoIndent<T>(this T obj, string compilerCode)
            where T : ICodeWriter
        {
            var indentBefore = obj.Indent;
            obj.Indent = 0;
            obj.WriteLine(compilerCode);
            obj.Indent = indentBefore;
            return obj;
        }
    }
}