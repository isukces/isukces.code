#if AMMY
using iSukces.Code.Compatibility.System.Windows;
using iSukces.Code.Compatibility.System.Windows.Controls;

namespace iSukces.Code.Compatibility.Telerik
{
    /// <summary>
    ///     Reflection only definition
    /// </summary>
    [EmitType("Telerik.Windows.Controls")]
    public class GridViewColumn : ItemsControl
    {
        public DataTemplate CellTemplate     { get; set; }
        public DataTemplate CellEditTemplate { get; set; }

        public GridViewEditTriggers EditTriggers { get; set; }
    }
}
#endif