namespace isukces.code.interfaces
{
    public static class StringExtensions
    {
        public static string Capitalize(this string x)
        {
            return x.Substring(0, 1).ToUpper() + x.Substring(1);
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
        ///     Koduje string do postaci stałej C#
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string CsCite(this string x)
        {
            const string quote = "\"";
            const string backslash = "\\";

            return quote + x
                       .Replace(backslash, backslash + backslash)
                       .Replace("\r", backslash + "r")
                       .Replace("\n", backslash + "n")
                       .Replace("\t", backslash + "t")
                       .Replace(quote, backslash + quote) + quote;
        }

        public static string UnCapitalize(this string x)
        {
            return x.Substring(0, 1).ToLower() + x.Substring(1);
        }
        
        public static string FirstLower(this string name) // !!!!!!
        {
            return name.Substring(0, 1).ToLower() + name.Substring(1);
        }


        public static string FirstUpper(this string name)
        {
            return name.Substring(0, 1).ToUpper() + name.Substring(1);
        }

    }
}