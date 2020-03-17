using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    public class AmmyStatement : IBaseData
    {
        public AmmyStatement(SourceSpan span)
        {
            Span = span;
        }

        public SourceSpan Span { get; }
    }
}