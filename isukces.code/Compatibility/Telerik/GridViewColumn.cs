using isukces.code.Compatibility.System.Windows;
using isukces.code.Compatibility.System.Windows.Controls;

namespace isukces.code.Compatibility.Telerik
{
    /// <summary>
    ///     Reflection only definition
    /// </summary>
    public class GridViewColumn : ItemsControl
    {
        public DataTemplate CellTemplate     { get; set; }
        public DataTemplate CellEditTemplate { get; set; }
    }
}