using System.Collections;

namespace isukces.code.Compatibility.System.Windows.Controls
{
    public class ItemsControl : Control
    {
        public IEnumerable ItemsSource       { get; set; }
        public string      DisplayMemberPath { get; set; }
    }
}