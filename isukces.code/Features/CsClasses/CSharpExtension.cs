#nullable enable
using System.Text;
using System.Xml.Linq;

namespace iSukces.Code
{
    public static class CSharpExtension
    {
        public static string GetCamelTerminalName(string name)
        {
            var s       = new StringBuilder();
            var toUpper = true;
            foreach (var i in name)
            {
                if (i == '_')
                {
                    toUpper = true;
                    continue;
                }

                s.Append(toUpper ? char.ToUpper(i) : i);
                toUpper = false;
            }

            return s.ToString();
        }

        public static string PropertyBackingFieldName(this string propertyName) => "_" + propertyName.FirstLower();

        public static string XmlEncode(this string x)
        {
            var e = new XElement("A", x);
            x = e.ToString();
            x = x.Substring(3, x.Length - 7);
            return x;
        }
    }
}