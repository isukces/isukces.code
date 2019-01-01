﻿using System.Collections.Generic;
using System.Linq;

namespace isukces.code.Typescript
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

        public override void WriteCodeTo(TsCodeFormatter formatter)
        {
            Introduction?.WriteCodeTo(formatter);
            if (Decorators != null && Decorators.Any())
                foreach (var i in Decorators)
                    i.WriteCodeTo(formatter);
            formatter.Open(string.Join(" ", GetClassHeader()));
            var left = Members.Count;
            foreach (var i in Members)
            {
                var line = --left != 0 ? i.GetCode() + "," : i.GetCode();
                formatter.Writeln(line);
            }
            formatter.Close();
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