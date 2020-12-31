using System.Collections.Generic;
using Irony.Parsing;

namespace Samples.Irony.AmmyGrammar
{
    sealed class MyFilter:TokenFilter
    {
        public override void Reset()
        {
            base.Reset();
        }

        protected override void OnSetSourceLocation(SourceLocation location)
        {
            base.OnSetSourceLocation(location);
        }

        public override IEnumerable<Token> BeginFiltering(ParsingContext context, IEnumerable<Token> tokens)
        {
            foreach (var i in tokens)
            {
                if (i.Terminal is NewLineTerminal)
                {
                    var previousTerminal = context.PreviousToken.Terminal;
                    if (previousTerminal.Name == "{")
                    {
                        continue; // just skip
                    }
                     
                }
                yield return i;
            }
        }
    }
}