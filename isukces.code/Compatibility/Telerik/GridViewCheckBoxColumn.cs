#if AMMY
using System;

namespace iSukces.Code.Compatibility.Telerik
{
    /// <summary>
    ///     Reflection only definition
    /// </summary>
    [EmitType("Telerik.Windows.Controls")]
    public class GridViewCheckBoxColumn : GridViewBoundColumnBase
    {
        public bool AutoSelectOnEdit { get; set; }
    }
 
    
    [Flags]
    [EmitType("Telerik.Windows.Controls")]
    public enum GridViewEditTriggers
    {
        /// <summary>
        /// Denotes that no action will put GridViewCell into edit mode.
        /// </summary>
        None = 1,
        /// <summary>
        /// Denotes that Single click on a cell will put it into edit mode.
        /// </summary>
        CellClick = 2,
        /// <summary>
        /// Denotes that click on a current cell will put it into edit mode.
        /// </summary>
        CurrentCellClick = 4,
        /// <summary>
        /// Denotes that F2 key on a cell will put it into edit mode.
        /// </summary>
        F2 = 8,
        /// <summary>
        /// Denotes that any text input key will put a cell into edit mode.
        /// </summary>
        TextInput = 16, // 0x00000010
        /// <summary>Combines default values.</summary>
        Default = TextInput | F2 | CurrentCellClick, // 0x0000001C
    }
}
#endif