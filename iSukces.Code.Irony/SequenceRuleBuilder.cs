#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Interfaces;
using iSukces.Code.Irony._codeSrc;

namespace iSukces.Code.Irony
{
    public sealed class MapInfo
    {
        public MapInfo(int index, NonTerminalInfo token = null, string propertyName = null,
            Func<ITypeNameResolver, string> propertyType = null)
        {
            Index        = index;
            Token        = token;
            PropertyName = propertyName;
            PropertyType = propertyType;
        }

        public int                             Index        { get; }
        public NonTerminalInfo                 Token        { get; }
        public string                          PropertyName { get; }
        public Func<ITypeNameResolver, string> PropertyType { get; }
    }

    public class SequenceRuleBuilder
    {
        public SequenceRuleBuilder()
        {
            _items = new List<Tup>();
        }

        public ICsExpression[] GetItems()
        {
            return _items.Select(a => a.Token).ToArray();
        }

        public MapInfo[] GetMap()
        {
            var l = new List<MapInfo>();
            for (var index = 0; index < _items.Count; index++)
            {
                var i = _items[index];
                if (string.IsNullOrEmpty(i.PropertyName))
                    continue;
                var mapInfo = new MapInfo(index, i.Token as NonTerminalInfo,
                    i.PropertyName,
                    ProcessToken?.Invoke(i.Token));
                l.Add(mapInfo);
            }

            return l.ToArray();
        }

        public SequenceRuleBuilder With(ICsExpression token, string name = null)
        {
            var tuple = new Tup(token, name);
            _items.Add(tuple);
            return this;
        }

        public SequenceRuleBuilder With(string token)
        {
            return With(new ToTermFunctionCall(token));
        }

        public Func<ICsExpression, Func<ITypeNameResolver, string>> ProcessToken { get; set; }


        private readonly List<Tup> _items;

        private struct Tup
        {
            public Tup(ICsExpression token, string propertyName)
            {
                Token        = token;
                PropertyName = propertyName;
            }

            public ICsExpression Token        { get; }
            public string        PropertyName { get; }
        }
    }
}
#endif