using System;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public class AncestorSource : IAmmyCodePieceConvertible
    {
        public AncestorSource(Type ancestorType)
        {
            _ancestorType = ancestorType;
        }

        public IAmmyCodePiece ToCodePiece(IConversionCtx ctx)
        {
            var txt = "$ancestor<" + ctx.TypeName(_ancestorType) + ">";
            return new SimpleAmmyCodePiece(txt);
        }

        private readonly Type _ancestorType;
    }
}