using System.Collections.Generic;
using iSukces.Code.Interfaces;

#nullable disable
namespace iSukces.Code.Irony
{
    public abstract partial class RuleBuilder
    {
        public abstract class Alternative : RuleBuilder, ICsExpression
        {
            public abstract IEnumerable<ICsExpression> GetAlternatives();

            public override string GetCode(ITypeNameResolver resolver) => GetCode(resolver, " | ", GetAlternatives());

            public abstract string GetDesc();


            public override string ToString() => GetDesc();

            public abstract TypeNameProviderEx AlternativeInterfaceName { get; set; }
        }
    }
}

