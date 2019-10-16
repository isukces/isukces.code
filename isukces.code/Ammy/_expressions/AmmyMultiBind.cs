using System.Linq;
using isukces.code.Compatibility.System.Windows.Data;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public partial class AmmyMultiBind  : IAmmyCodePieceConvertible
    {
        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            var builder = new AmmyObjectBuilder<XMultiBinding>();
            if (Mode != null)
                builder.WithProperty(a => a.Mode, Mode.Value);
            if (Converter != null)
                builder.WithProperty(a => a.Converter, Converter);
            if (ConverterCulture != null)
                builder.WithProperty(a => a.ConverterCulture, ConverterCulture);
            if (ConverterParameter != null)
                builder.WithProperty(a => a.ConverterParameter, ConverterParameter);
            if (Bindings.Any())
            {
                var ar = new AmmyArray();
                ar.Items.AddRange(Bindings);
                builder.WithProperty(a => a.Bindings, ar);
            }
            return builder.ToAmmyCode(ctx);
        }

    }
}