using System.Collections.Generic;
using iSukces.Code.Interfaces;
using iSukces.Code.Irony._codeSrc;

namespace iSukces.Code.Irony
{
    public abstract partial class RuleBuilder
    {
        public abstract class Alternative : RuleBuilder, ICsExpression
        {
            
            public override string GetCode(ITypeNameResolver resolver)
            {
                return GetCode(resolver, " | ", GetAlternatives());
            }
            
            public abstract IEnumerable<ICsExpression> GetAlternatives();

            public abstract string GetDesc();


            public override string ToString()
            {
                return GetDesc();
            }
            public abstract string AlternativeInterfaceName { get; }
        }
    }
}