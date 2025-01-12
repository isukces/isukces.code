using System.Collections.Generic;
using iSukces.Code.Interfaces;

#nullable disable
namespace iSukces.Code.Irony
{
    public abstract partial class RuleBuilder
    {
        public sealed class SequenceRule : RuleBuilder, IMap12
        {
            public SequenceRule(IReadOnlyList<SequenceItem> expressions, IReadOnlyList<MapInfo> map)
            {
                Expressions = expressions;
                Map         = map;
            }

            public IEnumerable<EnumerateTuple> Enumerate()
            {
                if (Map is null || Map.Count == 0)
                    for (var index = 0; index < Expressions.Count; index++)
                    {
                        var i = Expressions[index];
                        yield return new EnumerateTuple(index, null, i.Expression);
                    }
                else
                    foreach (var map in Map)
                    {
                        var i = Expressions[map.RuleItemIndex];
                        yield return new EnumerateTuple(map.AstIndex, map, i.Expression);
                    }
            }

            public override string GetCode(ITypeNameResolver resolver)
            {
                var tmp = GetCode(resolver, " + ", GetRuleExpressions());
                return tmp;
            }

            private IEnumerable<ICsExpression> GetRuleExpressions()
            {
                foreach (var i in Expressions)
                foreach (var j in i.GetAll())
                    yield return j;
            }

            public IReadOnlyList<SequenceItem> Expressions { get; }

            public IReadOnlyList<MapInfo> Map { get; }


            public struct EnumerateTuple
            {
                public EnumerateTuple(int astIndex, MapInfo map, ICsExpression expression)
                {
                    AstIndex   = astIndex;
                    Map        = map;
                    Expression = expression;
                }

                public int           AstIndex   { get; }
                public MapInfo       Map        { get; }
                public ICsExpression Expression { get; }
            }


            public sealed class SequenceItem
            {
                public SequenceItem(ICsExpression expression, IReadOnlyList<ICsExpression> hints)
                {
                    if (hints != null && hints.Count == 0)
                        hints = null;
                    Expression = expression;
                    Hints      = hints;
                }

                public IReadOnlyList<ICsExpression> GetAll()
                {
                    if (Hints is null)
                        return new[] {Expression};
                    var l = new List<ICsExpression>(1 + Hints.Count);
                    l.AddRange(Hints);
                    l.Add(Expression);
                    return l;
                }

                public ICsExpression                Expression { get; }
                public IReadOnlyList<ICsExpression> Hints      { get; }
            }
        }
    }
}

