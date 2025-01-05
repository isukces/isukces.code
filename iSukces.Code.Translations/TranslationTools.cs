namespace iSukces.Code.Translations
{
    public static class TranslationTools
    {
        public static string TranslationDecode(string? x)
        {
            const string backslash = "\\";

            return x?
                .Replace(backslash + "r", "\r")
                .Replace(backslash + "n", "\n")
                .Replace(backslash + "t", "\t")
                .Replace(backslash + backslash, backslash) ?? "";
        }

        public static string TranslationEncode(string? x)
        {
            const string backslash = "\\";

            return x?
                .Replace(backslash, backslash + backslash)
                .Replace("\r", backslash + "r")
                .Replace("\n", backslash + "n")
                .Replace("\t", backslash + "t") ?? "";
        }
    }
}
