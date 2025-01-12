#nullable disable
using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    public interface IBaseData
    {
        SourceSpan Span { get; }
    }
}
