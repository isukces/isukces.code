#if AMMY
using System.Collections.Generic;
using iSukces.Code.Compatibility.System.Windows;

namespace iSukces.Code.Compatibility.Telerik
{
    /// <summary>
    ///     Reflection only definition
    /// </summary>
    [EmitType("Telerik.Windows.Controls")]
    public class GridViewColumnGroup
    {
        public object Header { get; set; }
        
        public string Name { get; set; }

        public IList<GridViewColumnGroup> ChildGroups { get; set; }

        public DataTemplate HeaderTemplate { get; set; }

        public Style HeaderStyle { get; set; }
    }
}
#endif