using System.Collections.Generic;
using iSukces.Code.Interfaces;
using iSukces.Code.Irony._codeSrc;

namespace iSukces.Code.Irony
{
    public abstract partial class RuleBuilder
    {
        public sealed class SequenceRule : RuleBuilder, IMap12
        {
            public SequenceRule(IReadOnlyList<ICsExpression> expressions, IReadOnlyList<MapInfo> map)
            {
                Expressions = expressions;
                Map    = map;
            }

            public override string GetCode(ITypeNameResolver resolver)
            {
                return GetCode(resolver, " + ", Expressions);
            }

            public IReadOnlyList<ICsExpression> Expressions { get; }


            public IReadOnlyList<MapInfo> Map { get; }
        }
    }
}