#if AMMY
using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Compatibility.System.Windows.Data;
using iSukces.Code.Interfaces.Ammy;

namespace iSukces.Code.Ammy
{
    public partial class AmmyMultiBindingBuilder : IAmmyCodePieceConvertible
    {
        public AmmyMultiBindingBuilder()
        {
            Bindings = new List<object>();
        }

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            var builder = new AmmyObjectBuilder<XMultiBinding>();
            /*if (Converter != null)
                builder.WithProperty(a => a.Converter, Converter);
            if (Bindings.Any())
            {
                var ar = new AmmyArray();
                ar.Items.AddRange(Bindings);
                builder.WithProperty(a => a.Bindings, ar);
            }*/

            return builder.ToAmmyCode(ctx);
        }

        public AmmyMultiBindingBuilder WithBinding(AmmyBind bind)
        {
            Bindings.Add(bind);
            return this;
        }

       
        public object Converter { get; set; }
 
    }
}
#endif