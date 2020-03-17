using System.Collections.Generic;
using Irony.Parsing;
using JetBrains.Annotations;

namespace Bitbrains.AmmyParser
{
    public class AmmyCode : IBaseData
    {
        public AmmyCode(UsingStatements usings, SourceSpan span, IReadOnlyList<IAstStatement> statements)
        {
            Span   = span;
            Statements = statements ?? new IAstStatement[0];
            Usings = usings ?? new UsingStatements(new IAstUsingDirective[0], new SourceSpan(span.Location, 0));
        }

        [NotNull]
        public UsingStatements Usings { get; }

        public SourceSpan Span { get; }
        public IReadOnlyList<IAstStatement> Statements { get; }
    }
}