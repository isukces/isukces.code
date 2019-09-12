using isukces.code.Compatibility.System.Windows.Data;

namespace isukces.code.Compatibility.Telerik
{
    /// <summary>
    ///     Reflection only definition
    /// </summary>
    [EmitType("Telerik.Windows.Controls")]
    public class GridViewComboBoxColumn : GridViewBoundColumnBase
    {
        public XBinding ItemsSourceBinding      { get; set; }
        public string   SelectedValueMemberPath { get; set; }
    }
}