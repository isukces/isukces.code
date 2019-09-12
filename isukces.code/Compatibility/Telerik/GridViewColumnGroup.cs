using System.Collections.Generic;
using isukces.code.Compatibility.System.Windows;

namespace isukces.code.Compatibility.Telerik
{
    /// <summary>
    ///     Reflection only definition
    /// </summary>
    public class GridViewColumnGroup
    {
        public object Header { get; set; }
        
        public string Name   { get; set; }

        public IList<GridViewColumnGroup> ChildGroups { get; set; }

        public DataTemplate HeaderTemplate { get; set; }

        public Style HeaderStyle { get; set; }
    }
}