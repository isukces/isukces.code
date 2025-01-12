#nullable disable
using Irony.Interpreter;

namespace Bitbrains.AmmyParser
{
    public partial class AstAmmyBindSourceAncestor
    {
        protected override object DoEvaluate(ScriptThread thread)
        {
            return GetData(thread);
        }

        public IAstAmmyBindSourceSource GetData(ScriptThread thread)
        {
            var x =  base.DoEvaluate(thread);
            var xx = (object[])x;

            return new AstAmmyBindSourceAncestorData(
                Span,
                (FullQualifiedNameData)xx[1],
                (int?)xx[2]
            );
        }
    }
}
