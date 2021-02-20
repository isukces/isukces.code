using iSukces.Code.Compatibility.System.Windows;
using iSukces.Code.Compatibility.System.Windows.Data;

namespace iSukces.Code.Compatibility.Telerik
{
    /// <summary>
    ///     Reflection only definition
    /// </summary>
    [EmitType("Telerik.Windows.Controls")]
    public class GridViewBoundColumnBase : GridViewColumn
    {
        public XBinding DataMemberBinding { get; set; }
        public object   Header            { get; set; }
        public bool     IsReadOnly        { get; set; }
        public bool     IsSortable        { get; set; }

        public bool  IsFilterable                     { get; set; }
        public bool  ShowDistinctFilters              { get; set; }
        public bool  ShowFieldFilters                 { get; set; }
        public bool? ShouldGenerateFieldFilterEditors { get; set; }
        public bool  ShowFilterButton                 { get; set; }

        public bool           IsResizable      { get; set; }
        public GridViewLength Width            { get; set; }
        public string         ColumnGroupName  { get; set; }
        public TextAlignment  TextAlignment    { get; set; }
        public string         DataFormatString { get; set; }
    }
}