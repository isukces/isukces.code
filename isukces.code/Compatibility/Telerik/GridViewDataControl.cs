namespace iSukces.Code.Compatibility.Telerik
{
    /// <summary>
    ///     Reflection only definition
    /// </summary>
    [EmitType("Telerik.Windows.Controls.GridView")]
    public class GridViewDataControl : BaseItemsControl
    {
        public bool IsLocalizationLanguageRespected { get; set; }
    }
}