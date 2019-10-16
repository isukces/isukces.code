using System.Collections.Generic;
using isukces.code.Ammy;

namespace isukces.code.Compatibility.System.Windows.Data
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