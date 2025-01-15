using System;
using System.Collections.Generic;

namespace iSukces.Code;

public class TypeName : object
{
    public static TypeName FromString(string x) => new() { FullName = x };

    public static TypeName FromType(Type type, Dictionary<string, string> reductor) => new() { BaseType = type, Reductor = reductor };

    public static TypeName FromType(Type type) => new() { BaseType = type };

    /*
        string tn = TypeToString(type);
        string d = tn;
        if (d.IndexOf('<') >= 0)
            d = d.Substring(0, d.IndexOf('<'));

        d = d.Substring(0, d.LastIndexOf('.'));
        string n = tn.Substring( d.Length+1);
        return new TypeName() {Domain = d, Name = n}; */
    private static string TypeNameNoGeneric(Type t)
    {
        var tn = t.FullName;
        if (tn.IndexOf('`') >= 0)
            tn = tn.Substring(0, tn.IndexOf('`'));
        return tn;
    }

    public static string? TypeToString(Type type) => TypeToString(type, null);

    public static string? TypeToString(Type type, Dictionary<string, string>? reductor)
    {
        if (!type.IsGenericType)
        {
            var tn1 = type.FullName;
            if (reductor is null) return tn1;
            var d = tn1.Substring(0, tn1.LastIndexOf('.'));
            tn1 = tn1.Substring(tn1.LastIndexOf('.') + 1);
            if (reductor.ContainsKey(d))
                tn1 = reductor[d] + "." + tn1;
            return tn1;
        }

        var gt = type.GetGenericTypeDefinition();
        var tn = gt.FullName;
        tn = tn.Substring(0, tn.IndexOf("`"));

        var i = 0;
        foreach (var tt in type.GetGenericArguments())
        {
            tn += i == 0 ? "<" : ",";
            tn += TypeToString(tt, reductor);
            i++;
        }

        tn += ">";
        return tn;
    }

    public override string ToString() => FullName;

    #region Properties

    /// <summary>
    ///     typ, dla którego jest budowana nazwa
    /// </summary>
    public Type BaseType { get; set; }

    public string Domain
    {
        get
        {
            if (BaseType == (object?)null)
                return _domain;
            var tn = TypeNameNoGeneric(BaseType);

            var d = tn.Substring(0, tn.LastIndexOf('.'));
            if (Reductor is null) return d;
            if (Reductor.TryGetValue(d, out var domain))
                return domain;
            return d;
        }
        set => _domain = value;
    }

    /// <summary>
    ///     słownik przechowujący
    /// </summary>
    public string FullName
    {
        get
        {
            var d                           = Domain;
            if (!string.IsNullOrEmpty(d)) d += ".";
            return d + Name;
        }
        set
        {
            _domain = value.Substring(0, value.LastIndexOf('.'));
            _name   = value.Substring(value.LastIndexOf('.') + 1);
        }
    }

    public string Name
    {
        get
        {
            if (BaseType == (object?)null)
                return _name;
            var tn = TypeNameNoGeneric(BaseType);
            var d  = tn.Substring(0, tn.LastIndexOf('.'));
            var n  = tn.Substring(tn.LastIndexOf('.') + 1);
            if (BaseType.IsGenericType)
            {
                var i = 0;
                foreach (var t in BaseType.GetGenericArguments())
                {
                    n += i == 0 ? "<" : GlobalSettings.CommaSeparator;
                    var tn1 = new TypeName
                    {
                        BaseType = t,
                        Reductor = Reductor
                    };
                    n += tn1.FullName;
                    i++;
                }

                n += ">";
            }

            return n;
        }
        set => _name = value;
    }

    /// <summary>
    ///     typ, dla którego jest budowana nazwa
    /// </summary>
    public Dictionary<string, string> Reductor { get; set; }

    #endregion

    #region Fields

    private string _domain;

    private string _name;

    #endregion
}

