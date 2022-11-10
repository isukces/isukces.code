#if AMMY
namespace iSukces.Code.Compatibility.System.Windows
{
    [EmitType("System.Windows")]
    public class FrameworkElement
    {
        public Thickness         Margin            { get; set; }
        public VerticalAlignment VerticalAlignment { get; set; }
    }
}
#endif