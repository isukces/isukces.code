using Irony.Interpreter;

namespace Bitbrains.AmmyParser
{
    public partial class AstAmmyBind
    {
        protected override object DoEvaluate(ScriptThread thread)
        {
            var x = base.DoEvaluate(thread);
            return x;
        }

        public IAstAmmyPropertyValue GetData(ScriptThread thread) => throw new System.NotImplementedException();
    }
}