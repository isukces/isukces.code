using System.Collections.Generic;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Typescript
{
    public class TsEnum : TsNamespaceMember
    {
        public TsEnum(string name = null)
        {
            Name = name;
        }

        public TsEnum WithItem(string name, int value)
        {
            var item = new TsEnumItem
            {
                Name = name,
                Value = value
            };
            Members.Add(item);
            return this;
        }

        public override void WriteCodeTo(ITsCodeWriter writer)
        {
            WriteCommonHeaderCode(writer);
            writer.Open(string.Join(" ", GetClassHeader()));
            var left = Members.Count;
            foreach (var i in Members)
            {
                var line = --left != 0 ? i.GetCode() + "," : i.GetCode();
                writer.WriteLine(line);
            }
            writer.Close();
        }

        private IEnumerable<string> GetClassHeader()
        {
            if (IsExported)
                yield return "export";
            yield return "enum";
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