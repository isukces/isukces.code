#define _AutoCloseText
using System;
using JetBrains.Annotations;

namespace isukces.code.interfaces
{
    public interface ICodeWritter
    {
        void AppendText(string text);
        
        [NotNull]
        string Code { get; }
        
        [NotNull]
        ILangInfo LangInfo { get; }
        
        int       Indent   { get; set; }
    }

    public static class CodeFormatterExt
    {
        public static void DoWithKeepingIndent(this ICodeWritter _this, Action action)
        {
            var indentBefore = _this.Indent;
            action();
            if (_this.Indent == indentBefore) return;
            // some warning should be created here
            _this.Indent = indentBefore;
        }
        
        public static void Close(this ICodeWritter src, string customCloseText)
        {
            src.DecIndent();
            src.WriteLine(customCloseText);
        }

        public static void Close(this ICodeWritter src, bool addEmptyLine = false)
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
        public static void CloseBrackets(this ICodeWritter src)
        {
            src.DecIndent();
            src.WriteLine(src.LangInfo.CloseText);
        }

        public static void CloseNl(this ICodeWritter src)
        {
            src.Close(false);
            src.WriteLine(string.Empty);
        }


        public static void DecIndent(this ICodeWritter src)
        {
            src.Indent--;
        }

        [NotNull]
        public static string GetCodeTrim([NotNull] this ICodeWritter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            return writer.Code.Trim();
        }

        public static void IncIndent(this ICodeWritter src)
        {
            src.Indent++;
        }

        public static void Open(this ICodeWritter src, string x)
        {
#if AutoCloseText
            opening[src.Indent] = x;
#endif
            // Writeln(x + " " + LangInfo.OpenText);
            src.WriteLine(x);
            src.WriteLine(src.LangInfo.OpenText);
            src.Indent++;
        }

        public static void Open(this ICodeWritter src, string format, params object[] args)
        {
            src.Open(string.Format(format, args));
        }


        public static void OpenBrackets(this ICodeWritter src)
        {
            src.WriteLine(src.LangInfo.OpenText);
            src.IncIndent();
        }

        public static T WriteLine<T>(this T _this)
            where T : ICodeWritter
        {
            _this.AppendText("\r\n");
            return _this;
        }

        public static T WriteLine<T>(this T _this, string text)
            where T : ICodeWritter
        {
            _this.WriteIndent().AppendText(text + "\r\n");
            return _this;
        }


        public static T WriteLine<T>(this T src, string format, params object[] args)
            where T : ICodeWritter
        {
            var text = string.Format(format, args);
            return src.WriteLine(text);
        }

        public static void WritelineNoIndent(this ICodeWritter _this, string compilerCode)
        {
            var indentBefore = _this.Indent;
            _this.Indent = 0;
            _this.WriteLine(compilerCode);
            _this.Indent = indentBefore;
        }
    }
}