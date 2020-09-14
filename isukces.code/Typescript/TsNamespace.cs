using System.Collections.Generic;
using iSukces.Code.Interfaces;
using JetBrains.Annotations;

namespace iSukces.Code.Typescript
{
    public class TsNamespace : ITsCodeProvider, ITsExportable
    {
        public TsNamespace()
        {
        }

        public TsNamespace(string name)
        {
            Name = name;
        }

        public TsClass AddClass(string name)
        {
            var c = new TsClass
            {
                Name = name
            };
            Members.Add(c);
            return c;
        }

        [Pure]
        public string GetTypeName(AbstractType type)
        {
            return type.Namespace == Name ? type.Name : type.FullName;
        }

        public void WriteCodeTo(ITsCodeWriter writer)
        {
            writer.Open($"{(IsExport ? "export " : "")}namespace {Name}");
            if (Members != null)
                foreach (var i in Members)
                    i.WriteCodeTo(writer);
            writer.Close();
        }

        public string Name { get; set; }

        public bool IsExport { get; set; }

        public List<ITsCodeProvider> Members { get; set; } = new List<ITsCodeProvider>();
    }
}