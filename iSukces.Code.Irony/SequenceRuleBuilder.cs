using System;
using System.Collections.Generic;

namespace iSukces.Code.Irony
{
    public sealed class MapInfo
    {
        public MapInfo(int astIndex, int ruleItemIndex, NonTerminalInfo token = null, string propertyName = null,
            AstTypesInfoDelegate propertyType = null)
        {
            AstIndex      = astIndex;
            RuleItemIndex = ruleItemIndex;
            Token         = token;
            PropertyName  = propertyName;
            PropertyType  = propertyType;
        }

        public int                  AstIndex      { get; }
        public int                  RuleItemIndex { get; }
        public NonTerminalInfo      Token         { get; }
        public string               PropertyName  { get; }
        public AstTypesInfoDelegate PropertyType  { get; }
    }

    public class SequenceRuleBuilder
    {
        public SequenceRuleBuilder() => _items = new List<SeqTuple>();

        public MapInfo[] GetMap()
        {
            var l = new List<MapInfo>();
            for (int ruleItemIndex = 0, astIndex = 0; ruleItemIndex < _items.Count; ruleItemIndex++, astIndex++)
            {
                var item = _items[ruleItemIndex];

                if (DoesntProvideAstClass(item.Token))
                {
                    astIndex--;
                    continue;
                }

                if (string.IsNullOrEmpty(item.PropertyName))
                    continue;
                if (item.Token is ITokenNameSource tns)
                {
                    var aa = ProcessToken?.Invoke(tns);

                    var mapInfo = new MapInfo(astIndex, ruleItemIndex, item.Token as NonTerminalInfo,
                        item.PropertyName,
                        aa);
                    l.Add(mapInfo);
                }
                else
                {
                    throw new NotSupportedException();
                }
            }

            return l.ToArray();
        }

        public IReadOnlyList<RuleBuilder.SequenceRule.SequenceItem> GetRuleItems()
        {
            var result = new List<RuleBuilder.SequenceRule.SequenceItem>();
            for (var index = 0; index < _items.Count; index++)
            {
                var item = _items[index];
                if (item.Flag == SequenceFlags.None)
                {
                    result.Add(new RuleBuilder.SequenceRule.SequenceItem(item.Token, null));
                }
                else
                {
                    var                 hints = new List<ICsExpression>();
                    const SequenceFlags forb  = SequenceFlags.PreferShift | SequenceFlags.PreferReduce;
                    if ((item.Flag & forb) == forb)
                        throw new Exception("Invalid flags combinations " + forb);
                    if ((item.Flag & SequenceFlags.PreferShift) != 0)
                        hints.Add(PreferShiftHere);
                    if ((item.Flag & SequenceFlags.PreferReduce) != 0)
                        hints.Add(ReduceHere);
                    result.Add(new RuleBuilder.SequenceRule.SequenceItem(item.Token, hints));
                }
            }

            return result.ToArray();
        }

        public SequenceRuleBuilder With(ICsExpression token, string name = null, string propertyDescription = null)
        {
            var tuple = new SeqTuple(token, name, propertyDescription, SequenceFlags.None);
            _items.Add(tuple);
            return this;
        }

        public SequenceRuleBuilder With(ICsExpression token, SequenceFlags flag, string name = null,
            string propertyDescription = null)
        {
            var tuple = new SeqTuple(token, name, propertyDescription, flag);
            _items.Add(tuple);
            return this;
        }

        public SequenceRuleBuilder With(string token) => With(new ToTermFunctionCall(token));

        private bool DoesntProvideAstClass(ICsExpression token)
        {
            switch (token)
            {
                case NonTerminalInfo nti:
                    return !nti.AstClass.BuiltInOrAutocode;
                case ITokenNameSource tn:
                {
                    var name = tn.GetTokenName();
                    var info = GetTokenInfoByName(name);
                    if (info != null) return info.IsNoAst;

                    switch (tn.GetTokenNameIsNonterminal())
                    {
                        case TokenNameTarget.Unknown:
                            break;
                        case TokenNameTarget.Terminal:
                            return false; // terminal is not punctuation, has ast etc. AST is generated
                        case TokenNameTarget.Nonterminal:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    throw new ArgumentOutOfRangeException();
                }
                case WhiteCharCode _:
                    return true;
                default:
                    throw new NotSupportedException();
            }
        }

        public Func<ITokenNameSource, AstTypesInfoDelegate> ProcessToken       { get; set; }
        public Func<ITokenNameSource, TokenInfoResult>      GetTokenInfoByName { get; set; }
        private static readonly DirectCode PreferShiftHere = new DirectCode("this.PreferShiftHere()");
        private static readonly DirectCode ReduceHere = new DirectCode("this.ReduceHere()");

        private readonly List<SeqTuple> _items;

        private readonly struct SeqTuple
        {
            public SeqTuple(ICsExpression token, string propertyName, string propertyDescription, SequenceFlags flag)
            {
                Token               = token;
                PropertyName        = propertyName;
                PropertyDescription = propertyDescription;
                Flag                = flag;
            }

            public ICsExpression Token               { get; }
            public string        PropertyName        { get; }
            public string        PropertyDescription { get; }
            public SequenceFlags Flag                { get; }
        }

        public class TokenInfoResult
        {
            public SpecialTerminalKind? SpecialTerminal { get; set; }
            public bool                 IsNoAst         { get; set; }
        }
    }

    [Flags]
    public enum SequenceFlags
    {
        None = 0,
        PreferShift = 1,
        PreferReduce = 2
    }
}