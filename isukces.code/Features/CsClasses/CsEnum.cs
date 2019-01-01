using System;
using System.Collections.Generic;
using isukces.code.CodeWrite;
using isukces.code.interfaces;

namespace isukces.code
{
    public class CsEnum : ClassMemberBase
    {
        public CsEnum()
        {
            Items = new List<CsEnumItem>();
        }

        public void MakeCode(ICsCodeWritter writer)
        {
            writer.Open("public enum {0}", Name);
            if (Items != null)
            {
                var cnt = Items.Count;
                foreach (var i in Items)
                    i.MakeCode(writer, --cnt > 0);
            }
            writer.Close();
        }

        public string Name { get; set; }
        public IList<CsEnumItem> Items { get; set; }
        public Type DotNetType { get; set; }
    }
}