using Irony.Interpreter;

namespace Bitbrains.AmmyParser
{
    public partial class AstAmmyBind
    {
        protected override object DoEvaluate(ScriptThread thread)
        {
            return GetData(thread);
        }

        public IAstAmmyPropertyValue GetData(ScriptThread thread)
        {
            var x  = base.DoEvaluate(thread);
            var xx = (object[])x;
            return new AstAmmyBindData(Span,
                (string)xx[0],
                (IAstAmmyBindSourceSource)xx[1]
            );
        }
    }
}