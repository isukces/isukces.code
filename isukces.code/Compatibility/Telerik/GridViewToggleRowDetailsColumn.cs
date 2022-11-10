#if AMMY
namespace iSukces.Code.Compatibility.Telerik
{
    /// <summary>
    ///     Reflection only definition
    /// </summary>
    [EmitType("Telerik.Windows.Controls")]
    public class GridViewToggleRowDetailsColumn : GridViewColumn 
    {
        public GridViewToggleRowDetailsColumnExpandMode ExpandMode { get; set; }
    }
}
#endif