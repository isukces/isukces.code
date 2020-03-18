using Irony.Interpreter;

namespace Bitbrains.AmmyParser
{
    public partial class AstAmmyBindSourceThis
    {
        protected override object DoEvaluate(ScriptThread thread) => GetData(thread);

        public IAstAmmyBindSourceSource GetData(ScriptThread thread) => new AstAmmyBindSourceThisData();
    }

    public class AstAmmyBindSourceThisData : IAstAmmyBindSourceSource
    {
        public override string ToString() => "$this";
    }
}