using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace iSukces.Code.Ammy
{
    public class CodeEmbeder
    {
        public static string Embed(string target, string codeToEmbedd)
        {
            var open  = "// " + Limiter1 + " autocode begin " + Limiter2 + Environment.NewLine;
            var close = "// " + Limiter1 + " autocode end " + Limiter2 + Environment.NewLine;

            codeToEmbedd = codeToEmbedd?.Trim();

            if (string.IsNullOrEmpty(target))
            {
                if (string.IsNullOrEmpty(codeToEmbedd))
                    return string.Empty;
                return open + codeToEmbedd +Environment.NewLine+ close;
            }

            var l = new List<Find>();
            var m = Limiter.Match(target);
            while (m.Success)
            {
                var code = m.Groups[1].Value.TrimEnd('=').Trim();
                l.Add(new Find
                {
                    Start   = m.Groups[0].Index,
                    Length  = m.Groups[0].Length,
                    Code    = code,
                    IsBegin = AutoCodeBegin.Match(code).Success,
                    IsEnd   = AutoCodeEnd.Match(code).Success
                });
                m = m.NextMatch();
            }

            for (var idx = 1; idx < l.Count; idx++)
            {
                var b = l[idx - 1];
                var e = l[idx];
                if (!b.IsBegin || !e.IsEnd) continue;
                var sb = new StringBuilder();
                int conditions = 0;
                var firstCodePart = target.Substring(0, b.Start).TrimEnd();
                if (string.IsNullOrEmpty(firstCodePart))
                    conditions++; // brak kodu na początku
                else
                    sb.AppendLine(firstCodePart);
                sb.Append(open);
                if (string.IsNullOrEmpty(codeToEmbedd))
                    conditions++; // brak kodu osadzonego
                else
                    sb.AppendLine(codeToEmbedd);
                sb.Append(close);
                
                var restOfCode = target.Substring(e.Start + e.Length);
                var firstNl    = restOfCode.IndexOfAny(new[] {'\r', '\n'});
                if (firstNl >= 0)
                {
                    if (firstNl > 0)
                        restOfCode = restOfCode.Substring(firstNl);
                    restOfCode = restOfCode.TrimStart();
                    var cond1 = conditions;
                    if (string.IsNullOrEmpty(restOfCode))
                        conditions++;
                    else
                        sb.Append(restOfCode);
                    if (cond1 == 2)
                        return restOfCode; // we can always add markers at the beginning
                }
                else
                {
                    conditions++;
                }
                if (conditions>2)
                    return string.Empty;

                return sb.ToString();
            }

            return open + codeToEmbedd + Environment.NewLine + close + target;
        }

        public static Regex Limiter = new Regex(LimiterFilter,
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        public static Regex AutoCodeBegin =
            new Regex("^\\s*autocode\\s+begin\\s*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static Regex AutoCodeEnd =
            new Regex("^\\s*autocode\\s+end\\s*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public const string Limiter1 = "-----=====";
        public const string Limiter2 = "=====-----";
        public const string LimiterFilter = @"^\s*//\s*-{3,}={3,}\s*(.*)\s*={3,}-{3,}";

        private struct Find
        {
            public int    Start   { get; set; }
            public int    Length  { get; set; }
            public string Code    { get; set; }
            public bool   IsBegin { get; set; }
            public bool   IsEnd   { get; set; }
        }
    }
}