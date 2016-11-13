﻿using isukces.code.interfaces;

namespace isukces.code.CodeWrite
{
    public static class CsCodeWriterExtension
    {
        public static string GetIndent(this ICodeWriter _this)
        {
            return _this.Indent > 0 ? new string(' ', _this.Indent * 4) : "";
        }

        public static ICodeWriter WriteIndent(this ICodeWriter _this)
        {
            if (_this.Indent > 0)
                _this.AppendText(new string(' ', _this.Indent * 4));
            return _this;
        }


        public static ICodeWriter WriteLine(this ICodeWriter _this, string text)
        {

            _this.WriteIndent().AppendText(text + "\r\n");
            return _this;
        }

        public static ICodeWriter WriteLine(this ICodeWriter _this, string format, params object[] parameters)
        {
            _this.Indent++;
            _this.AppendText(string.Format(format + "\r\n", parameters));
            return _this;
        }

        public static ICodeWriter Open(this ICodeWriter _this, string text)
        {
            _this.WriteLine(text);
            _this.WriteLine("{");
            _this.Indent++;
            return _this;
        }

        public static ICodeWriter Open(this ICodeWriter _this, string format, params object[] parameters)
        {
            return _this.Open(string.Format(format, parameters));
        }


        public static ICodeWriter Close(this ICodeWriter _this)
        {
            _this.Indent--;
            _this.WriteLine("}");
            return _this;
        }


        public static ICodeWriter EmptyLine(this ICodeWriter _this, bool skip = false)
        {
            if (!skip)
                _this.AppendText("\r\n");
            return _this;
        }


        public static ICodeWriter OpenConstructor(this ICodeWriter _this, string x, string baseOrThis = null)
        {
            _this.WriteLine(x);
            if (!string.IsNullOrEmpty(baseOrThis))
            {
                _this.Indent++;
                _this.WriteLine(": " + baseOrThis);
                _this.Indent--;
            }
            _this.WriteLine("{");
            _this.Indent++;
            return _this;
        }


    }
}
