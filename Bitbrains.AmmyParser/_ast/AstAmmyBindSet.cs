#nullable disable
using System.Collections.Generic;
using Irony.Interpreter;
using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    public partial class AstAmmyBindSet
    {
        protected override object DoEvaluate(ScriptThread thread)
        {
            var x     = base.DoEvaluate(thread);
            var items = (IReadOnlyList<IAstAmmyBindSetItem>)x;
            return new AstAmmyBindSetData(Span, items);
        }

        protected override int[] GetMap()
        {
            return new[] {1};
        }
    }

    public class AstAmmyBindSetData : IBaseData
    {
        public AstAmmyBindSetData(SourceSpan span, IReadOnlyList<IAstAmmyBindSetItem> items)
        {
            Span  = span;
            Items = items;
        }

        public SourceSpan                         Span  { get; }
        public IReadOnlyList<IAstAmmyBindSetItem> Items { get; }
    }
}
