using System;
using System.Globalization;

namespace iSukces.Code.VsSolutions;

public readonly struct NumberOrText : IEquatable<NumberOrText>, IComparable<NumberOrText>, IComparable
{
    public NumberOrText(string text)
    {
        Text = text;
        if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n))
            Number = n;
    }

    public static bool operator ==(NumberOrText left, NumberOrText right)
    {
        return left.Equals(right);
    }

    public static bool operator >(NumberOrText left, NumberOrText right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(NumberOrText left, NumberOrText right)
    {
        return left.CompareTo(right) >= 0;
    }

    public static implicit operator NumberOrText(string? x)
    {
        return new NumberOrText(x);
    }

    public static implicit operator NumberOrText(int x)
    {
        return new NumberOrText(x.ToString(CultureInfo.InvariantCulture));
    }

    public static bool operator !=(NumberOrText left, NumberOrText right)
    {
        return !left.Equals(right);
    }

    public static bool operator <(NumberOrText left, NumberOrText right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(NumberOrText left, NumberOrText right)
    {
        return left.CompareTo(right) <= 0;
    }

    public int CompareTo(NumberOrText other)
    {
        if (Number is null)
        {
            if (other.Number is not null) return 1;
        }
        else
        {
            if (other.Number is null) return -1;
        }

        var numberComparison = Nullable.Compare(Number, other.Number);
        if (numberComparison != 0) return numberComparison;
        return string.Compare(Text, other.Text, StringComparison.Ordinal);
    }

    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        return obj is NumberOrText other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(NumberOrText)}");
    }

    public bool Equals(NumberOrText other)
    {
        return Text == other.Text;
    }

    public override bool Equals(object? obj)
    {
        return obj is NumberOrText other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Text.GetHashCode();
    }

    public override string ToString()
    {
        return Text;
    }

    public string Text => field ?? "";

    public int?   Number { get; }
}
