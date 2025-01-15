using System.Collections.Generic;

namespace iSukces.Code
{
    public static class LinqExtensions
    {
        internal static HashSet<T> ToHashSet<T>(this IEnumerable<T>? src)
        {
            return src is null ? new HashSet<T>() : new HashSet<T>(src);
        }
    }
}
