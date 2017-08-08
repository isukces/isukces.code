using System.Collections.Generic;
using System.Linq;

namespace isukces.code.Typescript
{
    public class TsEnum : TsClassOrEnum, ITsCodeProvider
    {
        public void WriteCodeTo(TsWriteContext cf)
        {
            if (Decorators != null && Decorators.Any())
                foreach (var i in Decorators)
                    i.WriteCodeTo(cf);
            cf.Formatter.Open(string.Join(" ", GetClassHeader()));
            var left = Members.Count;
            foreach (var i in Members)
            {
                var line = --left == 0 ? i.GetCode() + "," : i.GetCode();
                cf.Formatter.Writeln(line);
            }
            cf.Formatter.Close();
        }

        private IEnumerable<string> GetClassHeader()
        {
            if (IsExported)
                yield return "export";
            yield return "class";
            yield return Name;
        }

        public List<TsEnumItem> Members { get; set; } = new List<TsEnumItem>();
    }

    public class TsEnumItem
    {
        public string GetCode()
        {
            return Name + " = " + Value;
        }

        public string Name { get; set; }
        public int Value { get; set; }
    }
}