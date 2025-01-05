using System;
using JetBrains.Annotations;

namespace iSukces.Code.Translations;

public sealed class StaticPropertyReference : IEquatable<StaticPropertyReference>
{
    public StaticPropertyReference(Type type, string singletonPropertyName)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
        SingletonPropertyName =
            singletonPropertyName ?? throw new ArgumentNullException(nameof(singletonPropertyName));
    }

    public static bool operator ==(StaticPropertyReference left, StaticPropertyReference right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(StaticPropertyReference left, StaticPropertyReference right)
    {
        return !Equals(left, right);
    }

    public bool Equals(StaticPropertyReference? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Type == other.Type && SingletonPropertyName == other.SingletonPropertyName;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((StaticPropertyReference)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (Type.GetHashCode() * 397) ^ SingletonPropertyName.GetHashCode();
        }
    }

    public object GetSource()
    {
        var p = Type.GetProperty(SingletonPropertyName);
        return p.GetValue(null);
    }

    public Type   Type                  { get; }
    public string SingletonPropertyName { get; }
}