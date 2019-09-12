namespace isukces.code.Compatibility.System.Windows.Data
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