#if AMMY
using System;

namespace iSukces.Code.Ammy
{
    public struct AmmyCodePartsKey : IEquatable<AmmyCodePartsKey>, IComparable<AmmyCodePartsKey>, IComparable
    {
        public AmmyCodePartsKey(AmmyCodePartsKeyKind kind, string name)
        {
            Kind = kind;
            Name = name;
        }

        public static AmmyCodePartsKey Mixin(string mixinName)
        {
            return new AmmyCodePartsKey(AmmyCodePartsKeyKind.Mixin, mixinName);
        }

        public static bool operator ==(AmmyCodePartsKey left, AmmyCodePartsKey right)
        {
            return left.Equals(right);
        }

        public static bool operator >(AmmyCodePartsKey left, AmmyCodePartsKey right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(AmmyCodePartsKey left, AmmyCodePartsKey right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator !=(AmmyCodePartsKey left, AmmyCodePartsKey right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(AmmyCodePartsKey left, AmmyCodePartsKey right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(AmmyCodePartsKey left, AmmyCodePartsKey right)
        {
            return left.CompareTo(right) <= 0;
        }

        public int CompareTo(AmmyCodePartsKey other)
        {
            var kindComparison = Kind.CompareTo(other.Kind);
            if (kindComparison != 0) return kindComparison;
            return string.Compare(Name, other.Name, StringComparison.Ordinal);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            return obj is AmmyCodePartsKey other
                ? CompareTo(other)
                : throw new ArgumentException($"Object must be of type {nameof(AmmyCodePartsKey)}");
        }

        public bool Equals(AmmyCodePartsKey other)
        {
            return Kind == other.Kind && string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            return obj is AmmyCodePartsKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)Kind * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }

        public AmmyCodePartsKeyKind Kind { get; }
        public string               Name { get; }
    }
}
#endif