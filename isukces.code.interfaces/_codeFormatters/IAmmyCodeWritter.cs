namespace isukces.code.interfaces
{
    public interface IAmmyCodeWritter : ICodeWritter
    {
    }

    public static class AmmyCodeFormatterExt
    {
        public static void CloseArray(this IAmmyCodeWritter src)
        {
            src.Indent--;
            src.WriteLine("]");
        }

        public static void OpenArray(this IAmmyCodeWritter src, string text)
        {
            src.WriteLine(text + " [");
            src.Indent++;
        }
    }
}