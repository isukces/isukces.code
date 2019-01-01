using System.Collections.Generic;
using System.Linq;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Wpf.Ammy
{
    internal class AmmyArray : IAmmyExpression
    {
        public string GetAmmyCode(IConversionCtx ctx)
        {
            var converted = Items.Select(a => AmmyHelper.AnyObjectToString(a, ctx));
            return "[" + string.Join(", ", converted) + "]";
        }

        public List<object> Items { get; } = new List<object>();
    }
}