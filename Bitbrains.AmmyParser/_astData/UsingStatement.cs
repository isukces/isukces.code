using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    public class UsingStatement : IBaseData, IAstUsingDirective
    {
        public UsingStatement(SourceSpan span, FullQualifiedNameData name)
        {
            Name = name;
            Span = span;
        }

        public override string ToString()
        {
            return "using " + Name;
        }

        public FullQualifiedNameData Name { get; }
        public SourceSpan    Span { get; }
    }
}