using System;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public class AncestorBindingSource : IAmmyCodePieceConvertible
    {
        public AncestorBindingSource(Type ancestorType)
        {
            _ancestorType = ancestorType;
        }

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            var txt = "$ancestor<" + ctx.TypeName(_ancestorType) + ">";
            return new SimpleAmmyCodePiece(txt);
        }

        private readonly Type _ancestorType;
    }
}