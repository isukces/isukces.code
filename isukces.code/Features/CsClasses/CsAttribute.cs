using System;
using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Interfaces;

namespace iSukces.Code;

public class CsAttribute : ClassMemberBase, ICsAttribute
{
    public CsAttribute(string name)
    {
        Name = AttributableExt.CutAttributeSuffix(name);
    }

    public CsAttribute(CsType name)
        : this(name.AsString(false))
    {
    }

    public static CsAttribute Make<T>(ITypeNameResolver typeNameResolver)
    {
        return new CsAttribute(typeNameResolver.GetTypeName(typeof(T)));
    }

    public static implicit operator string(CsAttribute a)
    {
        return a.ToString();
    }

    public static implicit operator CsAttribute(string name)
    {
        return new CsAttribute(name);
    }

    private static string Encode(object? value)
    {
        switch (value)
        {
            case null:
                return "null";
            case bool aBool:
                return aBool ? "true" : "false";
            case int intValue:
                return intValue.ToCsString();
            case uint uintValue:
                return uintValue.ToCsString() + "u";
            case long longValue:
                return longValue.ToCsString() + "l";
            case ulong ulongValue:
                return ulongValue.ToCsString() + "ul";
            case byte byteValue:
                return "(byte)" + byteValue.ToCsString();
            case sbyte sbyteValue:
                return "(sbyte)" + sbyteValue.ToCsString();
            case double doubleValue:
                return doubleValue.ToCsString() + "d";
            case decimal decimalValue:
                return decimalValue.ToCsString() + "m";
            case float floatValue:
                return floatValue.ToCsString() + "f";
            case Guid gValue:
                if (gValue.Equals(Guid.Empty))
                    return "System.Guid.Empty";
                return "System.Guid.Parse(" + gValue.ToString("D").CsEncode() + ")";
            case string aString:
                return aString.CsEncode();
            case IDirectCode aDirectCode:
                return aDirectCode.Code;
            default:
                throw new NotSupportedException(value.GetType().ToString());
        }
    }

    private static string KeyValuePairToString(KeyValuePair<string, string> x)
    {
        return string.IsNullOrEmpty(x.Key) ? x.Value : $"{x.Key} = {x.Value}";
    }


    public override string ToString()
    {
        var values = _list.Select(KeyValuePairToString).ToArray();
        var name   = Name;
        if (name.EndsWith(AttributeSuffix, StringComparison.Ordinal))
            if (!name.Contains('.'))
            {
#if NET48
                name = name.Substring(name.Length - AttributeSuffixLength);
#else
                name = name[..^AttributeSuffixLength];
#endif
            }

        if (values.Length == 0)
            return name;
        return values.CommaJoin().Parentheses(name);
    }

    public CsAttribute WithArgument(object value)
    {
        return WithArgument("", value);
    }

    public CsAttribute WithArgument(string name, object value)
    {
        var sValue = Encode(value);
        return WithArgumentCode(name, sValue);
    }

    public CsAttribute WithArgumentCode(string valueCode)
    {
        return WithArgumentCode("", valueCode);
    }

    public CsAttribute WithArgumentCode(string name, string valueCode)
    {
        name = (name ?? "").Trim();
        _list.Add(new KeyValuePair<string, string>(name, valueCode));
        return this;
    }

    public string Name { get; set; }

    public string Code
    {
        get
        {
            if (_list is null || _list.Count == 0)
                return Name;
                
            return _list.Select(KeyValuePairToString)
                .CommaJoin()
                .Parentheses(Name);
        }
    }

    private static readonly int AttributeSuffixLength = AttributeSuffix.Length;

    private readonly List<KeyValuePair<string, string>> _list = new List<KeyValuePair<string, string>>();

    private const string AttributeSuffix = "Attribute";
}
