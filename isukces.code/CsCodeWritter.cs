using System.Collections.Generic;
using System.Linq;
using isukces.code.interfaces;

namespace isukces.code
{
    public class CsCodeWritter : CodeWritter, ICsCodeWritter
    {
        public CsCodeWritter() 
            : base(CsLangInfo.Instance)
        {            
        }

        public void AddNamespaces(string namespaceName)
        {
            Writeln("using {0};", namespaceName);
        }

        public void AddNamespaces(IEnumerable<string> namespaceName)
        {
            foreach (var x in namespaceName)
                AddNamespaces(x);
        }

        public CsCodeWritter SingleLineIf(string condition, string statement, string elseStatement = null)
        {
            Writeln("if (" + condition + ")");
            this.IncIndent();
            Writeln(statement);
            this.DecIndent();
            if (string.IsNullOrEmpty(elseStatement))
                return this;
            Writeln("else");
            this.IncIndent();
            Writeln(elseStatement);
            this.DecIndent();
            return this;
        }

        public CsCodeWritter WriteSingleLineSummary(string x)
        {
            var lines = x.Split('\r', '\n').Where(q => !string.IsNullOrEmpty(q?.Trim()));
            Writeln("/// <summary>");
            foreach (var line in lines)
                WriteSummary(line);
            Writeln("/// </summary>");
            return this;
        }


        public CsCodeWritter WriteSummary(string x)
        {
            // System.Xml.Linq.XObject el = new XElement("param", new XAttribute("name", p.Name), p.Description);
            // cs.Writeln("/// {0}", el);
            Writeln("/// {0}", x);
            return this;
        }
    }
}