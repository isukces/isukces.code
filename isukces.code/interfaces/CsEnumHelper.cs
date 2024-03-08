using System;
using System.Collections.Generic;
using System.Linq;

namespace iSukces.Code.Interfaces;

public sealed partial class CsEnumHelper
{
    public static EnumInfo Get(Type t)
    {
        if (_dict.TryGetValue(t, out var x))
            return x;
        return _dict[t] = Make(t);
    }

    private static EnumInfo Make(Type type)
    {
        var values    = Enum.GetValues(type).Cast<object>().ToArray();
        var ut        = Enum.GetUnderlyingType(type);
        var zerovalue = GetZeroValue(values, ut);
        var zero      = zerovalue?.ToString();
        return new EnumInfo(type, values, zero, ut);
    }

    private static readonly Dictionary<Type, EnumInfo> _dict = new Dictionary<Type, EnumInfo>();

    public class EnumInfo
    {
        public EnumInfo(Type type, object[] values, string zero, Type underlyingType)
        {
            Type           = type;
            Values         = values;
            Zero           = zero;
            UnderlyingType = underlyingType;
        }

        public IEnumerable<string> GetFlagStrings<T>(T value, string typeName) where T : Enum
        {
            var names = CsEnumHelper.GetFlagStrings(value, UnderlyingType, MaskValues, typeName);
            return names;
        }


        public Type     Type   { get; }
        public object[] Values { get; }

        public object[] MaskValues
        {
            get
            {
                if (!(_maskValues is null))
                    return _maskValues;
                var tmp = Values.Select(q =>
                    {
                        var map = GetBitsMap(q, UnderlyingType);
                        return new
                        {
                            Original = q,
                            Map      = map,
                            Count    = map?.Count ?? 0
                        };
                    }).Where(a => a.Count > 0)
                    .OrderByDescending(a => a.Count)
                    .ToArray();
                var allowBits = new HashSet<ushort>();
                foreach (var i in tmp)
                foreach (var j in i.Map)
                    allowBits.Add(j);

                var resultValues = new List<object>();
                foreach (var i in tmp)
                {
                    if (allowBits.Count == 0)
                        break;
                    foreach (var q in i.Map)
                    {
                        if (!allowBits.Contains(q)) continue;
                        resultValues.Add(i.Original);
                        if (i.Map.Count == 1)
                            // remove bit if mask consists of only one bit
                            // in fact i can remove when exactly one bit of mask is in allowBits
                            allowBits.Remove(i.Map[0]); 
                        break;
                    }
                }

                _maskValues = resultValues.ToArray();
                return _maskValues;
            }
        }

        public string Zero           { get; }
        public Type   UnderlyingType { get; }
        //private Lazy<IReadOnlyList<ushort>> _bitsMap;
        private object[] _maskValues;
    }
}