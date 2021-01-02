using iSukces.Code.AutoCode;

namespace iSukces.Code.Irony
{
    public interface IDoEvaluateHelper
    {
        bool GetDataClassConstructorArgument(ConstructorBuilder.Argument argument, bool lastChance, CsCodeWriter writer,
            out CsExpression constructorCallExpression);
    }
}