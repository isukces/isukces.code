using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace iSukces.Code.Irony
{
    public abstract partial class RuleBuilder
    {
        public class ListAlternative : Alternative, IMap12
        {
            public ListAlternative(string alternativeInterfaceName, IReadOnlyList<MapInfo> map,
                params ICsExpression[] alternatives)
            {
                AlternativeInterfaceName = alternativeInterfaceName;
                Map                      = map;
                Alternatives             = alternatives ?? new ICsExpression[0];
            }

            public bool Contains(TerminalName terminalName)
            {
                foreach (var a in Alternatives.OfType<TerminalName>())
                    if (a == terminalName)
                        return true;
                return false;
            }

            public override IEnumerable<ICsExpression> GetAlternatives() => Alternatives;

            public override string GetDesc()
            {
                var enumerable = Alternatives.Select(a =>
                {
                    switch (a)
                    {
                        case TokenInfo tokenInfo:
                            return tokenInfo.Name.Name;
                        case DirectCode directCode:
                            return directCode.Code;
                        case TerminalName tn:
                            return tn.Name;
                        case WhiteCharCode whc:
                            return whc.Code;
                        default:
                            return "?";
                    }
                });
                return "one of: " + string.Join(", ", enumerable);
            }


            [NotNull]
            public IReadOnlyList<ICsExpression> Alternatives { get; }

            public override string AlternativeInterfaceName { get; }

            public IReadOnlyList<MapInfo> Map { get; }
        }
    }

    public interface IMap12
    {
        IReadOnlyList<MapInfo> Map { get; }
    }
}