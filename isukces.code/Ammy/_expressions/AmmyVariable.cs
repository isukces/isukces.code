using iSukces.Code.Interfaces.Ammy;

namespace iSukces.Code.Ammy
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