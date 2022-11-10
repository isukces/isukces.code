#if AMMY
namespace iSukces.Code.Compatibility.System.Windows.Data
{
    /// <summary>
    ///     Reflection only enum
    /// </summary>
    [EmitType("System.Windows.Data", "BindingMode")]
    public enum XBindingMode
    {
        TwoWay,
        OneWay,
        OneTime,
        OneWayToSource,
        Default
    }
}
#endif