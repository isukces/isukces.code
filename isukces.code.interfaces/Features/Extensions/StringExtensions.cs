using System.Collections.Generic;
using System.Reflection;

namespace isukces.code.interfaces
{
    public static class StringExtensions
    {
        public static string Capitalize(this string x)
        {
            return x.Substring(0, 1).ToUpper() + x.Substring(1);
        }
        public static string UnCapitalize(this string x)
        {
            return x.Substring(0, 1).ToLower() + x.Substring(1);
        }

        /*
        public static List<T> GetAttributes<T>(this MemberInfo member, bool inherit)
        {
            var attributes = member.GetCustomAttributes(typeof(T), inherit);
            var list = new List<T>();
            foreach (var attribute in attributes)
                list.Add((T)attribute);
            return list;
        }
        */

      
        /// <summary>
        /// Koduje string do postaci stałej C#
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string CsCite(this string x)
        {
            return "\"" + x
                .Replace("\\", "\\\\")
                .Replace("\r", "\\\r")
                .Replace("\n", "\\\n")
                .Replace("\t", "\\\t")
                .Replace("\"", "\\\"") + "\"";
        }
    }
}
