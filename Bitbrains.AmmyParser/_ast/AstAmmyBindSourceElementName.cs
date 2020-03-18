using Irony.Interpreter;
using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    public partial class AstAmmyBindSourceElementName
    {
        public IAstAmmyBindSourceSource GetData(ScriptThread thread)
        {
            var tmp = base.DoEvaluate(thread);
            return new AstAmmyBindSourceElementNameData(Span, (string)tmp);
        }

        protected override object DoEvaluate(ScriptThread thread) => GetData(thread);
    }

    public class AstAmmyBindSourceElementNameData : IBaseData, IAstAmmyBindSourceSource
    {
        public AstAmmyBindSourceElementNameData(SourceSpan span, string elementName)
        {
            Span        = span;
            ElementName = elementName;
        }

        public override string ToString() => "\"" + ElementName + "\"";

        public SourceSpan Span        { get; }
        public string     ElementName { get; }
    }
}