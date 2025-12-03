using System;
using System.Collections.Generic;
using iSukces.Code.Interfaces;

namespace iSukces.Code;

public class CsMethodParameter : IComparable, IAttributable, IAnnotableByUser
{
    /// <summary>
    ///     Tworzy instancję obiektu
    ///     <param name="name">nazwa parametru</param>
    ///     <param name="type">typ parametru</param>
    ///     <param name="description">Opis</param>
    /// </summary>
    public CsMethodParameter(string name, CsType type = default, string? description = null)
    {
        Name        = name;
        Type        = type;
        Description = description;
    }
     
    /// <summary>
    ///     Realizuje operator ==
    /// </summary>
    /// <param name="left">lewa strona porównania</param>
    /// <param name="right">prawa strona porównania</param>
    /// <returns><c>true</c> jeśli obiekty są równe</returns>
    public static bool operator ==(CsMethodParameter? left, CsMethodParameter? right)
    {
        if (left is null)
            return ReferenceEquals(right, null);
        return left.Equals(right);
    }

    public static bool operator >(CsMethodParameter left, CsMethodParameter right) => left.CompareTo(right) > 0;

    public static bool operator >=(CsMethodParameter left, CsMethodParameter right) => left.CompareTo(right) >= 0;

    /// <summary>
    ///     Realizuje operator !=
    /// </summary>
    /// <param name="left">lewa strona porównania</param>
    /// <param name="right">prawa strona porównania</param>
    /// <returns><c>true</c> jeśli obiekty są różne</returns>
    public static bool operator !=(CsMethodParameter? left, CsMethodParameter? right) => !(left == right);

    public static bool operator <(CsMethodParameter left, CsMethodParameter right) => left.CompareTo(right) < 0;

    public static bool operator <=(CsMethodParameter left, CsMethodParameter right) => left.CompareTo(right) <= 0;

    /// <summary>
    ///     Realizuje interfejs IComparable
    /// </summary>
    public int CompareTo(CsMethodParameter other)
    {
        // Fail-safe check return
        // see Albahari, Joseph; Ben Albahari (2010-01-20). C# 4.0 in a Nutshell: The Definitive Reference (p. 258).
        if (Equals(other)) return 0;
        var tmp = string.Compare(Name, other.Name, StringComparison.Ordinal);
        return tmp != 0 ? tmp : string.Compare(Type.Modern, other.Type.Modern, StringComparison.Ordinal);
    }

    /// <summary>
    ///     Realizuje interfejs IComparable
    /// </summary>
    int IComparable.CompareTo(object? other)
    {
        if (other is not CsMethodParameter parameter)
            throw new InvalidOperationException("CompareTo: Object is not Parameter");
        return CompareTo(parameter);
    }

    /// <summary>
    ///     Sprawdza, czy wskazany obiekt jest równy bieżącemu
    /// </summary>
    /// <param name="other">obiekt do porównania z obiektem bieżącym</param>
    /// <returns><c>true</c> jeśli wskazany obiekt jest równy bieżącemu; w przeciwnym wypadku<c>false</c></returns>
    public bool Equals(CsMethodParameter? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(other, this))
            return true;
        return Name == other.Name && Type.Equals(other.Type);
    }

    /// <summary>
    ///     Sprawdza, czy wskazany obiekt jest równy bieżącemu
    /// </summary>
    /// <param name="other">obiekt do porównania z obiektem bieżącym</param>
    /// <returns><c>true</c> jeśli wskazany obiekt jest równy bieżącemu; w przeciwnym wypadku<c>false</c></returns>
    public override bool Equals(object? other) => other is CsMethodParameter parameter && Equals(parameter);

    /// <summary>
    ///     Zwraca kod HASH obiektu
    /// </summary>
    /// <returns>kod HASH obiektu</returns>
    public override int GetHashCode()
    {
        // Good implementation suggested by Josh Bloch
        var hash = 17;
        hash = hash * 31 + Name.GetHashCode();
        hash = hash * 31 + Type.GetHashCode();
        return hash;
    }

    public override string ToString()
    {
        var type       = Type.Modern;
        return !string.IsNullOrEmpty(_constValue)
            ? $"{type} {Name} = {_constValue}"
            : $"{type} {Name}";
    }

    /// <summary>
    /// </summary>
    public string? ConstValue
    {
        get => _constValue;
        set => _constValue = value?.Trim();
    }

    /// <summary>
    ///     nazwa parametru
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     typ parametru
    /// </summary>
    public CsType Type { get; set; }

    /// <summary>
    /// </summary>
    public ParameterCallTypes CallType { get; set; }

    /// <summary>
    ///     Opis
    /// </summary>
    public string? Description
    {
        get => _description;
        set => _description = value?.Trim() ?? string.Empty;
    }


    /// <summary>
    /// </summary>
    public bool IsReadOnly { get; set; }

    /// <summary>
    /// </summary>
    public bool UseThis { get; set; }

    /// <summary>
    /// </summary>
    public bool IsStatic { get; set; }

    public bool IsVolatile { get; set; }

    public IDictionary<string, object> UserAnnotations { get; } = new Dictionary<string, object>();

    /// <summary>
    ///     atrybuty
    /// </summary>
    public IList<ICsAttribute> Attributes
    {
        get => _attributes;
        set => _attributes = value ?? new List<ICsAttribute>();
    }

    private string? _constValue = string.Empty;
    private string? _description = string.Empty;
    private IList<ICsAttribute> _attributes = new List<ICsAttribute>();
}