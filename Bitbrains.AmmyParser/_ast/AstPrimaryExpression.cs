using Irony.Interpreter;

namespace Bitbrains.AmmyParser
{
    public partial class AstPrimaryExpression
    {
        IAstAmmyPropertyValue IAstAmmyPropertyValueProvider.GetData(ScriptThread thread) => throw new System.NotImplementedException();

        IAstExpression IAstExpressionProvider.GetData(ScriptThread thread) => throw new System.NotImplementedException();
    }
}