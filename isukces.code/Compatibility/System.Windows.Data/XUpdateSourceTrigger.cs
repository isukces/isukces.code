namespace iSukces.Code.Compatibility.System.Windows.Data
{
    /// <summary>
    ///     Reflection only enum
    /// </summary>
    [EmitType("System.Windows.Data", "UpdateSourceTrigger")]
    public enum XUpdateSourceTrigger
    {
        Default,
        PropertyChanged,
        LostFocus,
        Explicit
    }
}