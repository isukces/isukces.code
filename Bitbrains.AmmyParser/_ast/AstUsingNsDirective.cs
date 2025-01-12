#nullable disable
using Irony.Interpreter;

namespace Bitbrains.AmmyParser
{
    partial class AstUsingNsDirective
    { 

        protected override object DoEvaluate(ScriptThread thread)
        {
            var result = base.DoEvaluate(thread);
            var result2 = (FullQualifiedNameData)result;
            return new UsingStatement(Span, result2);
        }

        public IAstUsingDirective GetData(ScriptThread thread) => throw new System.NotImplementedException();
    }
}
