using System.Collections.Generic;

namespace iSukces.Code.Irony
{
    public abstract partial class RuleBuilder
    {
        public class OptionAlternative : Alternative
        {
            public OptionAlternative(TokenInfo info, TypeNameProviderEx alternativeInterfaceName)
            {
                Info                     = info;
                AlternativeInterfaceName = alternativeInterfaceName;
            }

            public override IEnumerable<ICsExpression> GetAlternatives()
            {
                yield return new DirectCode("Empty");
                yield return Info.Name;
            }

            public override string GetDesc() => "optional " + Info.Name;

            public TokenInfo Info { get; }

            public override TypeNameProviderEx AlternativeInterfaceName { get; set; }
        }
    }
}