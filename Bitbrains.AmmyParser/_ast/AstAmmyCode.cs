#nullable disable
using Irony.Interpreter;

namespace Bitbrains.AmmyParser
{
    public partial class AstAmmyCode
    {
        protected override object DoEvaluate(ScriptThread thread)
        {
            var evaluate = (object[])base.DoEvaluate(thread);
            var usings = (UsingStatements)evaluate[0];
            var statements = (IAstStatement[])evaluate[1];
            return new AmmyCode(usings, Span, statements);
        }
    }
}
