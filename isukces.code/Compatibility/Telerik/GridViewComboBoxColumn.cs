#if AMMY
using iSukces.Code.Compatibility.System.Windows.Data;

namespace iSukces.Code.Compatibility.Telerik
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
#endif