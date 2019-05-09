using System.Collections.Generic;

namespace isukces.code
{
    public static class LinqExtensions
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> src)
        {
            var r = new HashSet<T>();
            if (src == null)
                return r;
            foreach (var i in src)
                r.Add(i);
            return r;
        }
    }
}