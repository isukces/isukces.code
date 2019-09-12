using isukces.code.Compatibility.System.Windows;
using isukces.code.Compatibility.System.Windows.Controls;

namespace isukces.code.Compatibility.Telerik
{
    /// <summary>
    ///     Reflection only definition
    /// </summary>
    [EmitType("Telerik.Windows.Controls")]
    public class GridViewColumn : ItemsControl
    {
        public DataTemplate CellTemplate     { get; set; }
        public DataTemplate CellEditTemplate { get; set; }
    }
}