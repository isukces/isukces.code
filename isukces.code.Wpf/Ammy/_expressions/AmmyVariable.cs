using isukces.code.interfaces.Ammy;

namespace isukces.code.Wpf.Ammy
{
    public class AmmyVariable : IAmmyExpression
    {
        public AmmyVariable(string variableName)
        {
            VariableName = variableName;
        }

        public string GetAmmyCode(IConversionCtx ctx)
        {
            return "$" + VariableName;
        }

        public string VariableName { get; }
    }
}