using System;
using System.Collections.Generic;

namespace iSukces.Code
{
    internal static class PrvExtensions
    {
        public static TOut[] MapToArray<TIn, TOut>(this IReadOnlyList<TIn> src, Func<TIn, TOut> map)
        {
            if (src is null || src.Count == 0) return new TOut[0];
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
    }
}