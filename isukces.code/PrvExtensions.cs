using System;
using System.Collections.Generic;

namespace iSukces.Code
{
    internal static class PrvExtensions
    {
        public static TOut[] MapToArray<TIn, TOut>(this IReadOnlyList<TIn> src, Func<TIn, TOut> map)
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

        public static IReadOnlyList<T> CreateNewWithFirstElement<T>(this IReadOnlyList<T> els, T el)
        {
            if (els is null || els.Count == 0)
                return new[] {el};
            var r = new List<T>(els.Count + 1) {el};
            r.AddRange(els);
            return r;
        }
        public static IReadOnlyList<T> CreateNewWithLastElement<T>(this IReadOnlyList<T> els, T el)
        {
            if (els is null || els.Count == 0)
                return new[] {el};
            var r = new List<T>(els.Count + 1);
            r.AddRange(els);
            r.Add(el);
            return r;
        }
    }
}