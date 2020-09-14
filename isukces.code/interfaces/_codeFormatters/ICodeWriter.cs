#define _AutoCloseText
using System;
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

        [Obsolete("use just Close method")]
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




        public static void DoWithKeepingIndent(this ICodeWriter _this, Action action)
        {
            var indentBefore = _this.Indent;
            action();
            if (_this.Indent == indentBefore) return;
            // some warning should be created here
            _this.Indent = indentBefore;
        }

        [NotNull]
        public static string GetCodeTrim([NotNull] this ICodeWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            return writer.Code.Trim();
        }


        public static void Open(this ICodeWriter src, string x)
        {
#if AutoCloseText
            opening[src.Indent] = x;
#endif
            // Writeln(x + " " + LangInfo.OpenText);
            src.WriteLine(x);
            src.WriteLine(src.LangInfo.OpenText);
            src.Indent++;
        }

        public static void Open(this ICodeWriter src, string format, params object[] args)
        {
            src.Open(string.Format(format, args));
        }


        public static void OpenBrackets(this ICodeWriter src)
        {
            src.WriteLine(src.LangInfo.OpenText);
            src.IncIndent();
        }

        public static T DecIndent<T>(this T _this)
            where T : ICodeWriter
        {
            _this.Indent--;
            return _this;
        }

        public static T IncIndent<T>(this T _this)
            where T : ICodeWriter
        {
            _this.Indent++;
            return _this;
        }

        public static T WithOpen<T>(this T _this, string text)
            where T : ICodeWriter
        {
            _this.Open(text);
            return _this;
        }

        public static T WithClose<T>(this T _this)
            where T : ICodeWriter
        {
            _this.Close();
            return _this;
        }

        public static T WriteLine<T>(this T _this)
            where T : ICodeWriter
        {
            _this.Append("\r\n");
            return _this;
        }

        public static T WriteLine<T>(this T _this, string text)
            where T : ICodeWriter
        {
            _this.WriteIndent().Append(text + "\r\n");
            return _this;
        }

        public static T SplitWriteLine<T>(this T _this, string text)
            where T : ICodeWriter
        {
            if (string.IsNullOrEmpty(text))
                return _this;
            var query = from i in text.Split('\r', '\n')
                where !string.IsNullOrEmpty(i)
                select i.TrimEnd();
            foreach (var i in query)
                _this.WriteLine(i);
            return _this;
        }


        public static T WriteLine<T>(this T src, string format, params object[] args)
            where T : ICodeWriter
        {
            var text = string.Format(format, args);
            return src.WriteLine(text);
        }

        public static T WritelineNoIndent<T>(this T _this, string compilerCode)
            where T : ICodeWriter
        {
            var indentBefore = _this.Indent;
            _this.Indent = 0;
            _this.WriteLine(compilerCode);
            _this.Indent = indentBefore;
            return _this;
        }
    }
}