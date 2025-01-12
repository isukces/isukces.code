#nullable disable
using Irony.Interpreter;

namespace Bitbrains.AmmyParser
{
    public partial class AstUsingDirective : IAstUsingDirectiveProvider
    {
        public IAstUsingDirective GetData(ScriptThread thread)
        {
            var tmp = base.DoEvaluate(thread);
            if (tmp is object[] oArray)
                return (UsingStatement)oArray[0];
            return (UsingStatement)tmp;
        }

        protected override object DoEvaluate(ScriptThread thread) => GetData(thread);
    }
}
