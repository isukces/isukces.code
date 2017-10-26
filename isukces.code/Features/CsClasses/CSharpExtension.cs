using System.Xml.Linq;

namespace isukces.code
{
    public static class CSharpExtension
    {
        public static string CsharpCite(this string text)
        {
            return "\"" + text.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
        }

        public static string FirstLower(this string name) // !!!!!!
        {
            return name.Substring(0, 1).ToLower() + name.Substring(1);
        }


        public static string FirstUpper(this string name)
        {
            return name.Substring(0, 1).ToUpper() + name.Substring(1);
        }


        public static string PropertyBackingFieldName(this string propertyName)
        {
            return "_" + propertyName.FirstLower();
        }

        public static string XmlEncode(this string x)
        {
            var e = new XElement("A", x);
            x = e.ToString();
            x = x.Substring(3, x.Length - 7);
            return x;
        }
    }
}