using System.Collections.Generic;

namespace isukces.code
{
    public static class LinqExtensions
    {
        internal static HashSet<T> ToHashSet<T>(this IEnumerable<T> src)
        {
            return src == null ? new HashSet<T>() : new HashSet<T>(src);
        }
    }
}