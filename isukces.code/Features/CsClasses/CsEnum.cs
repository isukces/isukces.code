#region using

using System;
using System.Collections.Generic;
using isukces.code.CodeWrite;
using isukces.code.interfaces;

#endregion

namespace isukces.code
{
    public class CsEnum : ClassMemberBase
    {
        #region Constructors

        public CsEnum()
        {
            Items = new List<CsEnumItem>();
        }

        #endregion

        #region Instance methods

        public void MakeCode(ICodeWriter writer)
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

        #endregion

        #region Properties

        public string Name { get; set; }
        public IList<CsEnumItem> Items { get; set; }
        public Type DotNetType { get; set; }

        #endregion
 
    }
}