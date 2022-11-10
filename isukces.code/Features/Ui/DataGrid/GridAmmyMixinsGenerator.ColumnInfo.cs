using System;
using System.Collections.Generic;
using iSukces.Code.Ammy;

namespace iSukces.Code.Ui.DataGrid
{
    public abstract partial class GridAmmyMixinsGenerator
    {
        protected class ColumnInfo
        {
            public string Name { get; set; }

            public object HeaderSource { get; set; }
            public int?   Width        { get; set; }

            public string                     CategoryName     { get; set; }
            public bool                       AlignRight       { get; set; }
            public Type                       Type             { get; set; }
            public LookupInfo                 Lookup           { get; set; }
            public object                     CellTemplate     { get; set; }
            public object                     EditTemplate     { get; set; }
            public string                     DataFormatString { get; set; }
            public bool                       IsReadOnly       { get; set; }
            public bool                       IsSortable       { get; set; }
            public bool                       IsResizable      { get; set; }
#if AMMY
            public AmmyBindBuilder Binding { get; set; } = new AmmyBindBuilder(null);
#endif
            public Dictionary<string, object> CustomValues     { get; set; }
        }
    }
}