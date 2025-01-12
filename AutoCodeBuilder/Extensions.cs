#nullable disable
using System.Collections.Generic;

namespace AutoCodeBuilder
{
    static class Extensions
    {
        public static IReadOnlyList<T> CreateNewWithFirstElement<T>(this IReadOnlyList<T> els, T el)
        {
            if (els is null || els.Count == 0)
                return new[] {el};
            var r = new List<T>(els.Count + 1) {el};
            r.AddRange(els);
            return r;
        }
    }
}

