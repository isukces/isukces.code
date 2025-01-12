#nullable disable
using System.Collections.Generic;

namespace Bitbrains.AmmyParser
{
    public partial class AstUsingDirectives
    {
        protected override object MakeCollection(IReadOnlyList<IAstUsingDirective> items)
        {
            return new UsingStatements(items, Span);
        }
    }
}
