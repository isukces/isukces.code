using System;
using System.Collections.Generic;
using System.Text;
using iSukces.Translation;
using JetBrains.Annotations;

namespace iSukces.Code.Translations
{
    static class CsvHelper
    {
        public static List<string> DecodeLine(string text, char csvSeparator)
        {
            if (string.IsNullOrEmpty(text))
                return new List<string>();
            var l     = new List<string>();
            var state = CsvParseStates.ReadyForCollection;
            var sb    = new StringBuilder();

            void Flush()
            {
                l.Add(sb.ToString());
                sb.Clear();
            }

            const char quoteChar = '"';
            foreach (var ch in text)
            {
                switch (state)
                {
                    case CsvParseStates.ReadyForCollection:
                        if (ch == csvSeparator)
                        {
                            Flush();
                            continue;
                        }

                        if (ch == quoteChar)
                        {
                            state = CsvParseStates.CollectingTextInComma;
                            continue;
                        }

                        state = CsvParseStates.CollectingNormalText;
                        break;
                    case CsvParseStates.CollectingTextInComma:
                        if (ch == quoteChar)
                        {
                            state = CsvParseStates.CollectingTextInCommaQuoteFound;
                            continue;
                        }

                        break;
                    case CsvParseStates.CollectingNormalText:
                        if (ch == quoteChar)
                        {
                            var trimmed = sb.ToString().Trim();
                            if (trimmed != "")
                                throw new Exception($"Unexpected token '{ch}'");
                            state = CsvParseStates.CollectingTextInComma;
                            sb.Clear();
                            continue;
                        }

                        if (ch == csvSeparator)
                        {
                            Flush();
                            state = CsvParseStates.ReadyForCollection;
                            continue;
                        }

                        break;
                    case CsvParseStates.CollectingTextInCommaQuoteFound:
                        if (ch == quoteChar)
                        {
                            state = CsvParseStates.CollectingTextInComma;
                            break;
                        }

                        if (ch != csvSeparator)
                            throw new Exception($"Unexpected token '{ch}'");
                        Flush();
                        state = CsvParseStates.ReadyForCollection;
                        continue;
                    case CsvParseStates.ReadyForComma:
                        if (ch == csvSeparator)
                        {
                            state = CsvParseStates.ReadyForCollection;
                            continue;
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                sb.Append(ch);
            }

            switch (state)

            {
                case CsvParseStates.CollectingNormalText:
                case CsvParseStates.CollectingTextInCommaQuoteFound:
                case CsvParseStates.ReadyForCollection:
                    Flush();
                    break;
                case CsvParseStates.CollectingTextInComma:
                    throw new Exception("Unexpected end of text");
                case CsvParseStates.ReadyForComma:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return l;
        }

        public static string Encode(string[] x, char separator)
        {
            if (x == null) return string.Empty;
            var q = x.MapToArray(a => Encode(a, separator));
            return string.Join(separator.ToString(), q);
        }

        public static string Encode(string x, char separator)
        {
            if (string.IsNullOrEmpty(x)) return string.Empty;
            var sb        = new StringBuilder();
            var needQuote = false;
            foreach (var i in x)
                if (i == '"')
                {
                    sb.Append("\"\"");
                    needQuote = true;
                }
                else

                {
                    if (i == separator)
                        needQuote = true;
                    sb.Append(i);
                }

            if (needQuote)
                return "\"" + sb + "\"";
            return x;
        }

        private enum CsvParseStates
        {
            ReadyForCollection,
            CollectingTextInComma,
            CollectingNormalText,
            CollectingTextInCommaQuoteFound,
            ReadyForComma
        }
    }
}
