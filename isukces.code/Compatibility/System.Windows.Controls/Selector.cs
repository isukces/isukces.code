namespace isukces.code.Compatibility.System.Windows.Controls
{
    [EmitType("System.Windows.Controls.Primitives")]
    public class Selector : ItemsControl
    {
        public string SelectedValuePath { get; set; }
        public object SelectedValue     { get; set; }
    }
}