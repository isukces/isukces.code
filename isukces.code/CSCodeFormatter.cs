using System.Collections.Generic;
using System.Linq;

namespace isukces.code
{
    public class CSCodeFormatter : CodeFormatter
    {
        #region Constructors
        public CSCodeFormatter()
        {
            this.LangInfo = new CSLangInfo();
        }
        #endregion

        #region Methods
        public void AddNamespaces(string namespaceName)
        {
            Writeln("using {0};", namespaceName);
        }
        public void AddNamespaces(IEnumerable<string> namespaceName)
        {
            foreach (var x in namespaceName)
                AddNamespaces(x);
        }
        public string Cite(string x)
        {
            return "\"" + x.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
        }



        public void WriteSummary(string x)
        {
            // System.Xml.Linq.XObject el = new XElement("param", new XAttribute("name", p.Name), p.Description);
            // cs.Writeln("/// {0}", el);
            Writeln("/// {0}", x);
        }
        public void WriteSingleLineSummary(string x)
        {
            var lines = x.Split('\r', '\n').Where(q => !string.IsNullOrEmpty(q?.Trim()));
            Writeln("/// <summary>");
            foreach (var line in lines)
                WriteSummary(line);
            Writeln("/// </summary>");
        }
        #endregion
    }
}
