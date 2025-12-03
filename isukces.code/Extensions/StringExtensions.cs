using System;
using System.Globalization;
using System.Text;

namespace iSukces.Code;

public static class StringExtensions
{
    public static string CsEncode(this float value)
    {
        return value.ToString(CultureInfo.InvariantCulture) + "f";
    }

    public static string CsEncode(this double value)
    {
        return value.ToString(CultureInfo.InvariantCulture) + "d";
    }

    public static string CsEncode(this char i)
    {
        const string backslash = "\\";
        const string quote     = "\"";
        switch (i)
        {
            case '\\':
                return backslash + backslash;
            case '\r':
                return backslash + "r";
            case '\n':
                return backslash + "n";
            case '\t':
                return backslash + "t";
            case '\"':
                return backslash + quote;
            case '„':
            case '“':
            case '≥':
            case '≤':
            case '≈':
                return i.ToString();
            default:
                if (i < '\u2000')
                {
                    return i.ToString();
                }

                var ord = ((int)i).ToString("x4", CultureInfo.InvariantCulture);
                return "\\u" + ord;
        }
    }

    /// <param name="text"></param>
    extension(string text)
    {
        public string GetUntilSeparator(string separator, out string rest)
        {
            var idx = text.IndexOf(separator, StringComparison.Ordinal);
            if (idx < 0)
            {
                rest = string.Empty;
                return text;
            }

            rest = text[(idx + separator.Length)..];
            return text[..idx];
        }
    }


    /// <param name="text"></param>
    extension(string? text)
    {
        public string Capitalize()
        {
            if (text is null) return "";
            return text[..1].ToUpper() + text[1..];
        }

        /// <summary>
        ///     Koduje string do postaci stałej C#
        /// </summary>
        /// <returns></returns>
        public string CsEncode()
        {
            const string quote = "\"";
            // const string backslash = "\\";
            if (text is null)
                return "null";
            var sb = new StringBuilder();
            sb.Append(quote);
            foreach (var i in text)
            {
                var c = i.CsEncode();
                sb.Append(c);
            }

            sb.Append(quote);
            return sb.ToString();
        }

        /// <summary>
        ///     Koduje string do postaci stałej C#
        /// </summary>
        /// <returns></returns>
        public string CsVerbatimEncode()
        {
            if (text is null)
                return "null";

            const string quote = "\"";
            // const string backslash = "\\";
            var sb = new StringBuilder();
            sb.Append("@");
            sb.Append(quote);
            foreach (var c in text)
                if (c == '\"')
                    sb.Append(quote + quote);
                else
                    sb.Append(c);

            sb.Append(quote);
            return sb.ToString();
        }

        public string? Decamelize()
        {
            if (text is null)
                return null;
            var s = new StringBuilder();
            foreach (var i in text.Trim())
                if (s.Length == 0)
                {
                    s.Append(i);
                }
                else if (char.ToUpper(i) == i)
                {
                    s.Append(" ");
                    s.Append(char.ToLower(i));
                }
                else
                {
                    s.Append(i);
                }

            return s.ToString();
        }

        public string FirstLower() // !!!!!!
        {
            if (text is null) return "";
            return text[..1].ToLower() + text[1..];
        }

        public string FirstUpper()
        {
            if (text is null) return "";
            return text[..1].ToUpper() + text[1..];
        }

        public string UnCapitalize()
        {
            if (text is null) return "";
            return text[..1].ToLower() + text[1..];
        }


        /// <summary>
        ///     Adds escape to { and }
        /// </summary>
        /// <returns></returns>
        public string XamlEncode()
        {
            if (text is null) return string.Empty;
            var sb = new StringBuilder(Math.Max(text.Length, 64));
            foreach (var i in text)
                if (i == '{')
                    sb.Append("{}{");
                else
                    sb.Append(i);

            return sb.ToString();
        }
    }
}
