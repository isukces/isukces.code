using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public class AmmyVariable : IAmmyCodePieceConvertible
    {
        public AmmyVariable(string variableName)
        {
            VariableName = variableName;
        }

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            return new SimpleAmmyCodePiece("$" + VariableName);
        }

        public string VariableName { get; }
    }
}