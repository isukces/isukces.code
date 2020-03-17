using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    public class AstAmmyBindData : IBaseData, IAstAmmyPropertyValue
    {
        public AstAmmyBindData(SourceSpan span, string propertyName, IAstAmmyBindSourceSource source)
        {
            Span         = span;
            PropertyName = propertyName;
            Source       = source;
        }

        public override string ToString()
        {
            var name = "bind  \"" + PropertyName + "\"";
            if (Source is null)
                return name;
            return name + " from " + Source;
        }

        public SourceSpan               Span         { get; }
        public string                   PropertyName { get; }
        public IAstAmmyBindSourceSource Source       { get; }
    }
}