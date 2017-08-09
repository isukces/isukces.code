using System.Collections.Generic;
using System.Linq;

namespace isukces.code.Typescript
{
    public class TsEnum : TsNamespaceMember
    {
        public override void WriteCodeTo(TsWriteContext context)
        {
            if (Decorators != null && Decorators.Any())
                foreach (var i in Decorators)
                    i.WriteCodeTo(context);
            context.Formatter.Open(string.Join(" ", GetClassHeader()));
            var left = Members.Count;
            foreach (var i in Members)
            {
                var line = --left == 0 ? i.GetCode() + "," : i.GetCode();
                context.Formatter.Writeln(line);
            }
            context.Formatter.Close();
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