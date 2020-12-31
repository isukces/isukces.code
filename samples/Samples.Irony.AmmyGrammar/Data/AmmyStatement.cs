using Irony.Parsing;

namespace Samples.Irony.AmmyGrammar.Data
{
    public class AmmyStatement : IBaseData
    {
        public AmmyStatement(SourceSpan span) => Span = span;

        public SourceSpan Span { get; }
    }
}