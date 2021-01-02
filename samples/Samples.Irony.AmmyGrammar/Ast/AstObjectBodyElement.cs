using Irony.Interpreter;
using Samples.Irony.AmmyGrammar.Data;

namespace Samples.Irony.AmmyGrammar.Ast
{
    partial class AstObjectBodyElement
    {
        protected override object DoEvaluate(ScriptThread thread)
        {
            var t    = base.DoEvaluate(thread);
            var kind = GetNodeKind();
            return new AmmyObjectBodyElement(Span, t, kind);
        } 
    }
}