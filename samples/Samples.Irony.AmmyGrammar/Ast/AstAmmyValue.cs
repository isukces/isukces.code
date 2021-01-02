using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Samples.Irony.AmmyGrammar.Data;

namespace Samples.Irony.AmmyGrammar.Ast
{
    partial class AstAmmyValue
    {
        protected override object DoEvaluate(ScriptThread thread)
        {
            var node = ChildNodes[0];
            var v    = GetValue(thread, node);
            var kind = GetNodeKind();
            return new AmmyValue(Span, v, kind);
        }
    }
}