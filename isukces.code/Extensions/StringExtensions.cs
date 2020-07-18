﻿using System;
using System.Text;

namespace isukces.code
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
            const string backslash = "\\";
            if (x is null)
                return "null";
            return quote + x
                .Replace(backslash, backslash + backslash)
                .Replace("\r", backslash + "r")
                .Replace("\n", backslash + "n")
                .Replace("\t", backslash + "t")
                .Replace(quote, backslash + quote) + quote;
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
            const string backslash = "\\";
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

        public static string FirstLower(this string name) // !!!!!!
            =>
                name.Substring(0, 1).ToLower() + name.Substring(1);


        public static string FirstUpper(this string name) => name.Substring(0, 1).ToUpper() + name.Substring(1);

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