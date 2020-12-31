using System;
using System.Collections.Generic;
using iSukces.Code.Irony._codeSrc;

namespace iSukces.Code.Irony
{
    public sealed class MapInfo
    {
        public MapInfo(int index, NonTerminalInfo token = null, string propertyName = null,
            AstTypesInfoDelegate propertyType = null)
        {
            Index        = index;
            Token        = token;
            PropertyName = propertyName;
            PropertyType = propertyType;
        }

        public int                  Index        { get; }
        public NonTerminalInfo      Token        { get; }
        public string               PropertyName { get; }
        public AstTypesInfoDelegate PropertyType { get; }
    }

    public class SequenceRuleBuilder
    {
        public SequenceRuleBuilder() => _items = new List<SeqTuple>();

        public IReadOnlyList<RuleBuilder.SequenceRule.SequenceItem> GetItems()
        {
            var result = new RuleBuilder.SequenceRule.SequenceItem[_items.Count];
            for (var index = 0; index < _items.Count; index++)
            {
                var item = _items[index];
                if (item.Flag == SequenceFlags.None)
                {
                    result[index] = new RuleBuilder.SequenceRule.SequenceItem(item.Token, null);
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
                    result[index] = new RuleBuilder.SequenceRule.SequenceItem(item.Token, hints);
                }
            }

            return result;
        }

        public MapInfo[] GetMap()
        {
            var l = new List<MapInfo>();
            for (int index = 0, astIndex = 0; index < _items.Count; index++, astIndex++)
            {
                var i = _items[index];
                if (i.Token is WhiteCharCode)
                {
                    astIndex--;
                    continue;
                }

                if (string.IsNullOrEmpty(i.PropertyName))
                    continue;
                if (i.Token is ITerminalNameSource tns)
                {
                    var aa = ProcessToken?.Invoke(tns);

                    var mapInfo = new MapInfo(astIndex, i.Token as NonTerminalInfo,
                        i.PropertyName,
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

        public SequenceRuleBuilder With(ICsExpression token, string name = null, string propertyDescription = null)
        {
            var tuple = new SeqTuple(token, name, propertyDescription, SequenceFlags.None);
            _items.Add(tuple);
            return this;
        }

        public SequenceRuleBuilder With(ICsExpression token, SequenceFlags flag)
        {
            var tuple = new SeqTuple(token, null, null, flag);
            _items.Add(tuple);
            return this;
        }

        public SequenceRuleBuilder With(string token) => With(new ToTermFunctionCall(token));

        public Func<ITerminalNameSource, AstTypesInfoDelegate> ProcessToken { get; set; }
        private static readonly DirectCode PreferShiftHere = new DirectCode("this.PreferShiftHere()");
        private static readonly DirectCode ReduceHere = new DirectCode("this.ReduceHere()");


        private readonly List<SeqTuple> _items;

        private struct SeqTuple
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
    }

    [Flags]
    public enum SequenceFlags
    {
        None = 0,
        PreferShift = 1,
        PreferReduce = 2
    }
}