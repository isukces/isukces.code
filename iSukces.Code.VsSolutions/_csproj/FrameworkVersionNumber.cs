using System;
using System.Linq;

namespace iSukces.Code.VsSolutions;

public readonly struct FrameworkVersionNumber : IEquatable<FrameworkVersionNumber>, IComparable<FrameworkVersionNumber>, IComparable
{
    public FrameworkVersionNumber(string? value)
    {
        value = value?.Trim();
        if (string.IsNullOrEmpty(value)) return;
        Value = value;

        _parts = value.Split('.').Select(a => new NumberOrText(a)).ToArray();
    }

    public static bool operator ==(FrameworkVersionNumber left, FrameworkVersionNumber right)
    {
        return left.Equals(right);
    }

    public static bool operator >(FrameworkVersionNumber left, FrameworkVersionNumber right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(FrameworkVersionNumber left, FrameworkVersionNumber right)
    {
        return left.CompareTo(right) >= 0;
    }


    public static implicit operator FrameworkVersionNumber(string? x)
    {
        return new FrameworkVersionNumber(x);
    }

    public static bool operator !=(FrameworkVersionNumber left, FrameworkVersionNumber right)
    {
        return !left.Equals(right);
    }


    public static bool operator <(FrameworkVersionNumber left, FrameworkVersionNumber right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(FrameworkVersionNumber left, FrameworkVersionNumber right)
    {
        return left.CompareTo(right) <= 0;
    }

    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        return obj is FrameworkVersionNumber other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(FrameworkVersionNumber)}");
    }

    public int CompareTo(FrameworkVersionNumber other)
    {
        var otherParts = other._parts;
        if (otherParts is not null && _parts is not null)
        {
            var min = Math.Min(otherParts.Length, _parts.Length);
            for (var i = 0; i < min; i++)
            {
                var compare = _parts[i].CompareTo(otherParts[i]);
                if (compare != 0)
                    return compare;
            }
        }

        return string.Compare(Value, other.Value, StringComparison.Ordinal);
    }

    public bool Equals(FrameworkVersionNumber other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is FrameworkVersionNumber other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public string Value => field ?? "";

    private readonly NumberOrText[]? _parts;
}
