using System.Collections.Generic;
using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    public class UsingStatements : IBaseData
    {
        public UsingStatements(IReadOnlyList<IAstUsingDirective> items, SourceSpan span)
        {
            Items = items;
            Span  = span;
        }

        public IReadOnlyList<IAstUsingDirective> Items { get; }
        public SourceSpan                        Span  { get; }
    }
}