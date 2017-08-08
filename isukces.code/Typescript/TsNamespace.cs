﻿using System;
using System.Collections.Generic;

namespace isukces.code.Typescript
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

        public void WriteCodeTo(TsWriteContext cf)
        {
            cf.Formatter.Open($"{(IsExport ? "export " : "")}namespace {Name}");
            if (Members != null)
                foreach (var i in Members)
                    i.WriteCodeTo(cf);
            cf.Formatter.Close();
        }

        public string Name { get; set; }

        public bool IsExport { get; set; }

        public List<ITsCodeProvider> Members { get; set; } = new List<ITsCodeProvider>();
    }
}