using System;
using System.Collections.Generic;
using System.Linq;
using iSukces.Code.CodeWrite;
using iSukces.Code.Interfaces;

namespace iSukces.Code
{
    public class CsEnum : ClassMemberBase
    {
        public CsEnum(string name, params CsEnumItem[] items)
        {
            Name  = name;
            Items = items.ToList();
        }

        public CsEnum() => Items = new List<CsEnumItem>();

        public NamespaceAndName GetFullName() => new NamespaceAndName(GetNamespace(), Name);

        public string GetNamespace()
        {
            var owner = Owner;
            while (true)
                switch (owner)
                {
                    case null:
                    case CsFile _:
                        return string.Empty;
                    case CsNamespace ns:
                        return ns.Name;
                    case CsClass cl:
                        owner = cl.Owner;
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(owner));
                }
        }

        public void MakeCode(ICsCodeWriter writer)
        {
            CsClass.WriteAttributes(writer, Attributes);
            var def = $"{Visibility.ToCsCode()} enum {Name}";
            var ut  = UnderlyingType?.Trim();
            if (!string.IsNullOrEmpty(ut))
                def += ": " + ut;
            writer.Open(def);
            if (Items != null)
            {
                var cnt = Items.Count;
                foreach (var i in Items)
                    i.MakeCode(writer, --cnt > 0);
            }

            writer.Close();
        }

        public IClassOwner Owner { get; set; }

        public string            Name  { get; set; }
        public IList<CsEnumItem> Items { get; set; }

        public string UnderlyingType { get; set; }
    }
}