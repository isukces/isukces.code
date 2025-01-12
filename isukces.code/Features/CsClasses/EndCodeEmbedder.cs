using System;
using System.Text;
using System.Text.RegularExpressions;

namespace iSukces.Code
{
    public class EndCodeEmbedder
    {
        public static string Append(string? target, string codeToAppend, string end)
        {
            codeToAppend = codeToAppend?.Trim();
            end = end.Trim();
            var fullDelimiter = $"// {start} {end} {Limiter2}{Environment.NewLine}";

            if (string.IsNullOrEmpty(target))
            {
                if (string.IsNullOrEmpty(codeToAppend))
                    return string.Empty;
                return fullDelimiter + codeToAppend + Environment.NewLine;
            }

            const string spaces         = "\\s+";
            const string spacesOptional = "\\s*";
            var          delFilter      = end.Replace(" ", spaces);
            var          filter         = $"^{spacesOptional}//{spacesOptional}{start}{spacesOptional}{delFilter}{spacesOptional}{Limiter2}";
            var          limiterSearch  =new Regex(filter, RegexOptions.Multiline);

            var m = limiterSearch.Match(target);
            if (!m.Success) {
                if (string.IsNullOrEmpty(codeToAppend))
                    return target;
                return  target.TrimEnd() + Environment.NewLine 
                    + fullDelimiter
                    + codeToAppend + Environment.NewLine;
            }

            var a  = target.Substring(0, m.Index);
            var sb = new StringBuilder();
            sb.Append(a.TrimEnd());
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            
            sb.Append(fullDelimiter);
            sb.Append(codeToAppend);
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }


        private const string start = "-----=====";
        private const string Limiter2 = "=====-----";
    }
}

