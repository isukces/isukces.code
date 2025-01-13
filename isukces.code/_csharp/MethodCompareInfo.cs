using System;
using System.Reflection;

namespace iSukces.Code;

public sealed class MethodCompareInfo : IEquatable<MethodCompareInfo>, IComparable<MethodCompareInfo>
{
    public MethodCompareInfo(MethodInfo mi)
    {
        _mi = mi;
    }

    public int Kind
    {
        get
        {
            if (_mi.IsConstructor)
                return 0;
            if (_mi.IsPublic)
                return 1;
            if (_mi.IsPrivate)
                return 3;
            return 2;
        }
    }

    public string Name => _mi.Name ?? "";

    public bool IsStatic => _mi.IsStatic;

    private readonly MethodInfo _mi;

    public int CompareTo(MethodCompareInfo? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        var comparisonResult = IsStatic.CompareTo(other.IsStatic);
        if (comparisonResult != 0) return comparisonResult;
        comparisonResult = Kind.CompareTo(other.Kind);
        if (comparisonResult != 0) return comparisonResult;
        return StringComparer.Ordinal.Compare(Name, other.Name);
    }

    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (ReferenceEquals(this, obj)) return 0;
        return obj is MethodCompareInfo other
            ? CompareTo(other)
            : throw new ArgumentException("Object must be of type MethodCompareInfo");
    }

    public override bool Equals(object? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return other is MethodCompareInfo otherCasted && Equals(otherCasted);
    }

    public bool Equals(MethodCompareInfo? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return IsStatic == other.IsStatic
               && Kind == other.Kind
               && StringComparer.Ordinal.Equals(Name, other.Name);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (Kind * 397 ^ StringComparer.Ordinal.GetHashCode(Name)) * 2 + (IsStatic ? 1 : 0);
        }
    }

    public static bool operator !=(MethodCompareInfo? left, MethodCompareInfo? right)
    {
        return !Equals(left, right);
    }

    public static bool operator <(MethodCompareInfo? left, MethodCompareInfo? right)
    {
        if (Equals(left, right)) return false;
        if (left is null) // null.CompareTo(NOTNULL) = -1
            return true;
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(MethodCompareInfo? left, MethodCompareInfo? right)
    {
        if (Equals(left, right)) return true;
        if (left is null) // null.CompareTo(NOTNULL) = -1
            return true;
        return left.CompareTo(right) <= 0;
    }

    public static bool operator ==(MethodCompareInfo? left, MethodCompareInfo? right)
    {
        return Equals(left, right);
    }

    public static bool operator >(MethodCompareInfo? left, MethodCompareInfo? right)
    {
        if (Equals(left, right)) return false;
        if (left is null) // null.CompareTo(NOTNULL) = -1
            return false;
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(MethodCompareInfo? left, MethodCompareInfo? right)
    {
        if (Equals(left, right)) return true;
        if (left is null) // null.CompareTo(NOTNULL) = -1
            return false;
        return left.CompareTo(right) >= 0;
    }


}
