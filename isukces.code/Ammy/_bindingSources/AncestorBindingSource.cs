using System;
using System.Globalization;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public class AncestorBindingSource : IAmmyCodePieceConvertible
    {
        public AncestorBindingSource(Type ancestorType, int? level = null)
        {
            _ancestorType = ancestorType;
            _level        = level;
        }

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            var txt = "$ancestor<" + ctx.TypeName(_ancestorType) + ">";
            if (_level != null)
                txt += "(" + _level.Value.ToString(CultureInfo.InvariantCulture) + ")";
            return new SimpleAmmyCodePiece(txt);
        }

        private readonly Type _ancestorType;
        private readonly int? _level;
    }
}