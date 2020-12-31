using Irony.Interpreter;
using Samples.Irony.AmmyGrammar.Data;

namespace Samples.Irony.AmmyGrammar.Ast
{
    partial class AstMainObjectStatement
    {
        protected override object DoEvaluate(ScriptThread thread)
        {
            var baseObjectType = (AmmyDomainStyleName)BaseObjectType.Evaluate(thread);
            var fullTypeName              = (string)FullTypeName.Evaluate(thread);

            var r = new AmmyMainObjectStatement(Span, baseObjectType, fullTypeName);
            var t = base.DoEvaluate(thread);
            return r;
        }
    }

    partial class AstAmmyProgram
    {
        protected override object DoEvaluate(ScriptThread thread)
        {
            var usings           = Usings.Evaluate(thread);
            var objectDefinition = ObjectDefinition.Evaluate(thread);
            var r = new AmmyProgram(Span,
                (AmmyUsingStatementCollection)usings,
                (AmmyMainObjectStatement)objectDefinition);
            var t = base.DoEvaluate(thread);
            return r;
        }
    }
}