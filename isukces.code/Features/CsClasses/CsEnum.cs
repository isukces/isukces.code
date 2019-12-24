﻿using System.Collections.Generic;
using System.Linq;
using isukces.code.interfaces;

namespace isukces.code
{
    public class CsEnum : ClassMemberBase
    {
        public CsEnum(string name, params CsEnumItem[] items)
        {
            Name  = name;
            Items = items.ToList();
        }

        public CsEnum() => Items = new List<CsEnumItem>();

        public void MakeCode(ICsCodeWriter writer)
        {
            CsClass.WriteAttributes(writer, Attributes);
            writer.Open(Visibility.ToCsCode() + " enum {0}", Name);
            if (Items != null)
            {
                var cnt = Items.Count;
                foreach (var i in Items)
                    i.MakeCode(writer, --cnt > 0);
            }

            writer.Close();
        }

        public string            Name  { get; set; }
        public IList<CsEnumItem> Items { get; set; }
    }
}