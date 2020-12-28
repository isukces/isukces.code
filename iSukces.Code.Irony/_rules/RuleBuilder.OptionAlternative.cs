using System.Collections.Generic;
using iSukces.Code.Irony._codeSrc;

namespace iSukces.Code.Irony
{
    public abstract partial class RuleBuilder
    {
        public class OptionAlternative : Alternative
        {
            public OptionAlternative(TerminalName name, string alternativeInterfaceName)
            {
                Name             = name;
                AlternativeInterfaceName = alternativeInterfaceName;
            }

            public override IEnumerable<ICsExpression> GetAlternatives()
            {
                yield return new DirectCode("Empty");
                yield return Name;
            }

            public override string GetDesc()
            {
                return "optional " + Name.Name;
            }

            public TerminalName Name { get; }

            public override string AlternativeInterfaceName { get; }
        }
    }
}