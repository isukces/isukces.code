using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace iSukces.Code;

public static class CSharpExtension
{
    extension(string text)
    {
        public string GetCamelTerminalName()
        {
            var s       = new StringBuilder();
            var toUpper = true;
            foreach (var i in text)
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

        public string PropertyBackingFieldName() => "_" + text.FirstLower();

        public string XmlEncode()
        {
            var e = new XElement("A", text);
            text = e.ToString();
            text = text.Substring(3, text.Length - 7);
            return text;
        }

        public string[] SplitToLines()
        {
            var lines = text.Replace("\r\n", "\n")
                .Trim()
                .Split('\n');
            if (lines.Length == 1 && lines[0].Trim().Length == 0)
                return [];
            return lines;
        }
    }
}