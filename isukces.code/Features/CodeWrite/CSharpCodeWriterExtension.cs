using System;
using isukces.code.interfaces;

namespace isukces.code.CodeWrite
{
    public static class CSharpCodeWriterExtension
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

        public static ICodeWriter OpenBrackets(this ICodeWriter _this)
        {
            _this.WriteLine("{");
            _this.Indent++;
            return _this;
        }
        public static ICodeWriter CloseBrackets(this ICodeWriter _this)
        {
            _this.Indent--;
            _this.WriteLine("}");
            return _this;
        }



        public static ICodeWriter WriteLine(this ICodeWriter _this, string format, params object[] parameters)
        {
            // _this.Indent++;
            if (_this.Indent > 0)
                _this.AppendText(GetIndent(_this));
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


        public static void DoWithKeepingIndent(this ICodeWriter _this, Action action)
        {
            var indentBefore = _this.Indent;
            action();
            if (_this.Indent == indentBefore) return;
            // some warning should be created here
            _this.Indent = indentBefore;
        }
        
        public static void OpenCompilerIf(this ICodeWriter _this, string directive)
        {
            if (!string.IsNullOrEmpty(directive)) 
                WritelineNoIndent(_this, "#if " + directive);
        }
        
        public static void CloseCompilerIf(this ICodeWriter _this, string directive)
        {
            if (!string.IsNullOrEmpty(directive)) 
                WritelineNoIndent(_this, "#endif");
        }

        private static void WritelineNoIndent(this ICodeWriter _this, string compilerCode)
        {
            var indentBefore = _this.Indent;
            _this.Indent = 0;
            _this.WriteLine(compilerCode);
            _this.Indent = indentBefore;
        }
    }
}
