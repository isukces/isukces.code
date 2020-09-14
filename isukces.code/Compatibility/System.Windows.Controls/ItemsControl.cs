using System.Collections;

namespace iSukces.Code.Compatibility.System.Windows.Controls
{
    [EmitType("System.Windows.Controls")]
    public class ItemsControl : Control
    {
        public IEnumerable ItemsSource       { get; set; }
        public string      DisplayMemberPath { get; set; }
    }
}