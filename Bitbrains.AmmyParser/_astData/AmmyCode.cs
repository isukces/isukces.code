using System;
using System.Collections.Generic;
using Irony.Parsing;
using JetBrains.Annotations;

namespace Bitbrains.AmmyParser
{
    public class AmmyCode : IBaseData
    {
        public AmmyCode(UsingStatements usings, SourceSpan span, IReadOnlyList<IAstStatement> statements)
        {
            Span       = span;
            Statements = statements ?? Array.Empty<IAstStatement>();
            Usings     = usings ?? new UsingStatements(Array.Empty<IAstUsingDirective>(), new SourceSpan(span.Location, 0));
        }

        [NotNull]
        public UsingStatements Usings { get; }

        public SourceSpan Span { get; }
        public IReadOnlyList<IAstStatement> Statements { get; }
    }
}