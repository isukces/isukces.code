#if OBSOLETE
namespace iSukces.Code.AutoCode
{
    public static class WriterExtension
    {
       
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
    }
}
#endif