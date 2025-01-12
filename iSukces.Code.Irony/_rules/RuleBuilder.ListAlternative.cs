using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

#nullable disable
namespace iSukces.Code.Irony
{
    public abstract partial class RuleBuilder
    {
        public class ListAlternative : Alternative, IMap12
        {
            public ListAlternative(TypeNameProviderEx alternativeInterfaceName, IReadOnlyList<MapInfo> map,
                params ICsExpression[] alternatives)
            {
                AlternativeInterfaceName = alternativeInterfaceName;
                Map                      = map;
                Alternatives             = alternatives ?? XArray.Empty<ICsExpression>();
            }

            public bool Contains(TokenName tokenName)
            {
                foreach (var a in Alternatives.OfType<TokenName>())
                    if (a == tokenName)
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
                        case TokenName tn:
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

            public override TypeNameProviderEx AlternativeInterfaceName { get; set; }

            public IReadOnlyList<MapInfo> Map { get; }
        }
    }

    public interface IMap12
    {
        IReadOnlyList<MapInfo> Map { get; }
    }
}

