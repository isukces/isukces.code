using System;

namespace isukces.code.Ui.DataGrid
{
    public abstract partial class GridAmmyMixinsGenerator
    {
        protected class ColumnInfo
        {
            public string     Name              { get; set; }
            public string     Header            { get; set; }
            public int?       Width             { get; set; }
            public string     CategoryName      { get; set; }
            public bool       AlignRight        { get; set; }
            public Type       Type              { get; set; }
            public LookupInfo Lookup            { get; set; }
            public object     CellTemplate      { get; set; }
            public object     EditTemplate      { get; set; }
            public string     DataMemberBinding { get; set; }
            public string     DataFormatString  { get; set; }
            public bool       IsReadOnly        { get; set; }
        }
    }
}