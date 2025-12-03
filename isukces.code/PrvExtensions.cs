using System;
using System.Collections.Generic;

namespace iSukces.Code;

internal static class PrvExtensions
{
    extension<TIn>(IReadOnlyList<TIn>? src)
    {
        public TOut[] MapToArray<TOut>(Func<TIn, TOut> map)
        {
            if (src is null || src.Count == 0) return XArray.Empty<TOut>();
            var result = new TOut[src.Count];
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var i = 0; i < src.Count; i++)
            {
                var element = src[i];
                var value   = map(element);
                result[i] = value;
            }

            return result;
        }

        public IReadOnlyList<TIn> CreateNewWithFirstElement(TIn el)
        {
            if (src is null || src.Count == 0)
                return [el];
            var r = new List<TIn>(src.Count + 1) { el };
            r.AddRange(src);
            return r;
        }

        public IReadOnlyList<TIn> CreateNewWithLastElement(TIn el)
        {
            if (src is null || src.Count == 0)
                return [el];
            var r = new List<TIn>(src.Count + 1);
            r.AddRange(src);
            r.Add(el);
            return r;
        }
    }


    public static string CommaJoin(this IEnumerable<string> texts)
    {
        return string.Join(GlobalSettings.CommaSeparator, texts);
    }

    public static string CommaJoin<T>(this IEnumerable<T> objects)
    {
        return string.Join(GlobalSettings.CommaSeparator, objects);
    }

    extension(string text)
    {
        public string Parentheses() => $"({text})";
        public string Parentheses(string prefix) => $"{prefix}({text})";
        public string TriangleBrackets() => $"<{text}>";
        public string New(string prefix) => $"new {prefix}({text})";
    }
}
