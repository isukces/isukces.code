namespace isukces.code.interfaces
{
    public interface ICsCodeWritter : ICodeWritter
    {
    }

    public static class CsCodeFormatterExt
    {
        public static ICodeWritter OpenConstructor(this ICodeWritter _this, string x, string baseOrThis = null)
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

        
        public static void CloseCompilerIf(this ICsCodeWritter _this, string directive)
        {
            if (!string.IsNullOrEmpty(directive))
                _this.WritelineNoIndent("#endif");
        }

        public static void CloseCompilerIf(this ICsCodeWritter _this, IConditional conditional)
        {
            _this.CloseCompilerIf(conditional?.CompilerDirective);
        }

        public static T EmptyLine<T>(this T _this, bool skip = false)
            where T : ICodeWritter
        {
            if (!skip)
                _this.AppendText("\r\n");
            return _this;
        }

        public static string GetIndent(this ICodeWritter _this)
        {
            return _this.Indent > 0 ? new string(' ', _this.Indent * 4) : "";
        }


        public static void OpenCompilerIf(this ICsCodeWritter _this, string directive)
        {
            if (!string.IsNullOrEmpty(directive))
                _this.WritelineNoIndent("#if " + directive);
        }

        public static void OpenCompilerIf(this ICsCodeWritter _this, IConditional conditional)
        {
            _this.OpenCompilerIf(conditional?.CompilerDirective);
        }


        public static T WriteIndent<T>(this T _this)
            where T : ICodeWritter
        {
            if (_this.Indent > 0)
                _this.AppendText(new string(' ', _this.Indent * 4));
            return _this;
        }
    }
}