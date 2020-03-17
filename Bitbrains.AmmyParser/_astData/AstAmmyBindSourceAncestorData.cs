using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    public class AstAmmyBindSourceAncestorData : IAstAmmyBindSourceSource, IBaseData
    {
        public AstAmmyBindSourceAncestorData(SourceSpan span, FullQualifiedNameData ancestorType, int? level)
        {
            Span         = span;
            AncestorType = ancestorType;
            Level        = level;
        }

        public override string ToString()
        {
            return $"$ancestor<{AncestorType}>({Level})";
        }

        public SourceSpan            Span         { get; }
        public FullQualifiedNameData AncestorType { get; }
        public int?                  Level        { get; }
    }
}