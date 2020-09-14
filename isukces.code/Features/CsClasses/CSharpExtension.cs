using System.Xml.Linq;
using iSukces.Code.Interfaces;

namespace iSukces.Code
{
    public static class CSharpExtension
    {
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