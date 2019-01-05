using isukces.code.interfaces;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public class AmmyVariableDefinition : IAmmyCodePieceConvertible
    {
        public AmmyVariableDefinition(string name, string value)
        {
            _name  = name;
            _value = value;
        }

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            return new SimpleAmmyCodePiece(_name + " = " + _value.CsEncode());
        }

        private readonly string _name;
        private readonly string _value;
    }
}