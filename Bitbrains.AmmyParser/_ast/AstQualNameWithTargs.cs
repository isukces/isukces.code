using Irony.Interpreter;

namespace Bitbrains.AmmyParser
{
    public partial class AstQualNameWithTargs
    {
        protected override object DoEvaluate(ScriptThread thread)
        {
            var baseResult = (object[])base.DoEvaluate(thread);
            var first      = (string)baseResult[0];
            var rest = (FullQualifiedNameData)baseResult[1];
            var parts = rest.GetPartsWithPrefix(first);
            return new FullQualifiedNameData(Span, parts);
        }
    }
}