using System.Collections.Generic;
using iSukces.Code.Ammy;

namespace iSukces.Code.Compatibility.System.Windows.Data
{
    /// <summary>
    ///     Reflection only class
    /// </summary>
    [EmitType("System.Windows.Data", "Binding")]
    public class XBinding
    {
    }


    [EmitType("System.Windows.Data", "MultiBinding")]
    public partial class XMultiBinding
    {
        public XMultiBinding()
        {
            Bindings = new List<object>();
        }
    }
}