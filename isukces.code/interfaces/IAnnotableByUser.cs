using System.Collections.Generic;

namespace iSukces.Code.Interfaces
{
    public interface IAnnotableByUser
    {
        /// <summary>
        ///     Additional information used by custom generators
        /// </summary>
        IDictionary<string, object> UserAnnotations { get; }
    }

    public static class AnnotableByUserExt
    {
        public static T? GetAnnotation<T>(this IAnnotableByUser an, string key)
        {
            if (!an.UserAnnotations.TryGetValue(key, out var value)) return default;
            return value is T tt ? tt : default;
        }
    }
}
