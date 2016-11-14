namespace isukces.code.AutoCode
{
    public static class WriterExtension
    {
        #region Static Methods

        public static void CloseBrackets(this IDirectCodeWriter src)
        {
            src.DecIndent();
            src.WriteLn("}");
        }

        public static void DecIndent(this IDirectCodeWriter src)
        {
            src.ChangeIndent(-1);
        }

        public static void IncIndent(this IDirectCodeWriter src)
        {
            src.ChangeIndent(1);
        }

        public static void OpenBrackets(this IDirectCodeWriter src)
        {
            src.WriteLn("{");
            src.IncIndent();
        }

        public static void WriteLnF(this IDirectCodeWriter src, string f, params object[] p)
        {
            src.WriteLn(string.Format(f, p));
        }

        #endregion
    }
}