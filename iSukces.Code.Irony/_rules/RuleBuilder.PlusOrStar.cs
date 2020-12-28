using iSukces.Code.Interfaces;
using iSukces.Code.Irony._codeSrc;

namespace iSukces.Code.Irony
{
    public abstract partial class RuleBuilder
    {
        public sealed class PlusOrStar : RuleBuilder
        {
            public PlusOrStar(ICsExpression delimiter, ICsExpression element, bool isPlus, TerminalName output)
            {
                Delimiter = delimiter ?? new DirectCode("null");
                Element   = element;
                IsPlus    = isPlus;
                Output    = output;
            }

            public override string GetCode(ITypeNameResolver resolver)
            {
                var args = new[]
                {
                    Output,
                    Delimiter,
                    Element
                };
                var argsCode   = GetCode(resolver, ", ", args);
                var methodName = IsPlus ? "MakePlusRule" : "MakeStarRule";
                return methodName + "(" + argsCode + ")";
            }

            public ICsExpression Delimiter { get; }
            public ICsExpression Element   { get; }
            public bool          IsPlus    { get; }
            public TerminalName  Output    { get; }
        }
    }
}