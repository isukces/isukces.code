using isukces.code.Compatibility.System.Windows;
using isukces.code.Compatibility.System.Windows.Data;

namespace isukces.code.Compatibility.Telerik
{
    /// <summary>
    ///     Reflection only definition
    /// </summary>
    [EmitType("Telerik.Windows.Controls")]
    public class GridViewBoundColumnBase : GridViewColumn
    {
        public XBinding       DataMemberBinding { get; set; }
        public object         Header            { get; set; }
        public bool           IsReadOnly        { get; set; }
        public GridViewLength Width             { get; set; }
        public string         ColumnGroupName   { get; set; }
        public TextAlignment  TextAlignment     { get; set; }
    }
}