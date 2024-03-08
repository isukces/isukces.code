using System;
using System.Globalization;
using System.Text;
using JetBrains.Annotations;

namespace iSukces.Code
{
    public static class StringExtensions
    {
        public static string Capitalize(this string x) => x.Substring(0, 1).ToUpper() + x.Substring(1);

        /// <summary>
        ///     Koduje string do postaci stałej C#
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string CsEncode(this string x)
        {
            const string quote     = "\"";
            // const string backslash = "\\";
            if (x is null)
                return "null";
            var sb = new StringBuilder();
            sb.Append(quote);
            foreach (var i in x)
            {
                var c = i.CsEncode();
                sb.Append(c);
            }

            sb.Append(quote);
            return sb.ToString();
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
                    else
                    {
                        var ord = ((int)i).ToString("x4", CultureInfo.InvariantCulture);
                        return "\\u" + ord;
                    }
            }
        }

        /// <summary>
        ///     Koduje string do postaci stałej C#
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string CsVerbatimEncode(this string x)
        {
            if (x is null)
                return "null";

            const string quote     = "\"";
            // const string backslash = "\\";
            var          sb        = new StringBuilder();
            sb.Append("@");
            sb.Append(quote);
            foreach (var c in x)
                if (c == '\"')
                    sb.Append(quote + quote);
                else
                    sb.Append(c);

            sb.Append(quote);
            return sb.ToString();
        }

        public static string Decamelize(this string name)
        {
            if (name is null)
                return null;
            var s = new StringBuilder();
            foreach (var i in name.Trim())
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

        public static string FirstLower([NotNull] this string name) // !!!!!!
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            return name.Substring(0, 1).ToLower() + name.Substring(1);
        }


        public static string FirstUpper(this string name) => name.Substring(0, 1).ToUpper() + name.Substring(1);

        public static string GetUntilSeparator(this string x, string separator, out string rest)
        {
            var idx = x.IndexOf(separator);
            if (idx < 0)
            {
                rest = string.Empty;
                return x;
            }

            rest = x.Substring(idx + separator.Length);
            return x.Substring(0, idx);
        }

        public static string UnCapitalize(this string x) => x.Substring(0, 1).ToLower() + x.Substring(1);

        /// <summary>
        ///     Adds escape to { and }
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string XamlEncode(this string x)
        {
            var sb = new StringBuilder(Math.Max(x.Length, 64));
            foreach (var i in x)
                if (i == '{')
                    sb.Append("{}{");
                else
                    sb.Append(i);

            return sb.ToString();
        }
    }
}

// Console.Write("");