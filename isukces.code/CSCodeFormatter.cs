using System.Collections.Generic;

namespace isukces.code
{
    public class CSCodeFormatter:CodeFormatter
    {
        #region Constructors
        public CSCodeFormatter()
        {
            this.LangInfo = new CSLangInfo();
        }
        #endregion

        #region Methods
        public void AddNamespaces(string namespaceName) {
            Writeln("using {0};", namespaceName);
        }
        public void AddNamespaces(IEnumerable<string> namespaceName)
        {
            foreach(var x in namespaceName)
                AddNamespaces(x);
        }
        public string Cite(string x)
        {
            return "\"" + x.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
        }


        
        public void WriteSummary(string x)
        {
            ///                 System.Xml.Linq.XObject el = new XElement("param", new XAttribute("name", p.Name), p.Description);
            // cs.Writeln("/// {0}", el);
            Writeln("/// {0}", x);
        }
        public void WriteSingleLineSummary(string x)
        {
            Writeln("/// <summary>");
            WriteSummary(x);
            Writeln("/// </summary>");
        }
        #endregion
    }
}
