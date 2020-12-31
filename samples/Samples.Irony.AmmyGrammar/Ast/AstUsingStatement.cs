using Irony.Interpreter;
using Samples.Irony.AmmyGrammar.Data;

namespace Samples.Irony.AmmyGrammar.Ast
{
    public partial class AstUsingStatement
    {
        protected override object DoEvaluate(ScriptThread thread)
        {
            var namespaceName = (AmmyDomainStyleName)NamespaceName.Evaluate(thread);
            return new AmmyUsingStatement(Span, namespaceName);
        }
    }

    partial class AstUsingStatementCollection
    {
        protected override object DoEvaluate(ScriptThread thread)
        {
            var items = EvaluateItems(thread);
            return new AmmyUsingStatementCollection(Span, items);
        }
    }

    partial class AstDomainStyleName
    {
        protected override object DoEvaluate(ScriptThread thread)
        {
            var items = EvaluateItems();
            return new AmmyDomainStyleName(Span, items);
        }
    }

    /*
    partial class AstObjectBodyElementCollection
    {
        protected override object DoEvaluate(ScriptThread thread)
        {
            var items = EvaluateItems(thread);
            return new AmmyObjectBodyElementCollection(Span, items);
        }
    }*/
}