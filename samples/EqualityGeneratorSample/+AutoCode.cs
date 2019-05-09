// ReSharper disable All
using System;

// ReSharper disable once CheckNamespace
namespace EqualityGeneratorSample
{
    partial class SampleClass : isukces.code.AutoCode.IAutoEquatable<SampleClass>
    {
        public override bool Equals(object other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return other is SampleClass otherCasted && Equals(otherCasted);
        }

        public bool Equals(SampleClass other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return System.StringComparer.Ordinal.Equals(FirstName, other.FirstName)
                && System.StringComparer.Ordinal.Equals(LastName, other.LastName)
                && BirthDate.Equals(other.BirthDate)
                && (OtherDate is null ? other.OtherDate is null : OtherDate.Value.Equals(other.OtherDate));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = System.StringComparer.Ordinal.GetHashCode(FirstName ?? string.Empty);
                hashCode = (hashCode * 397) ^ System.StringComparer.Ordinal.GetHashCode(LastName ?? string.Empty);
                hashCode = (hashCode * 397) ^ BirthDate.GetHashCode();
                hashCode = (hashCode * 397) ^ OtherDate?.GetHashCode()  ?? 0;
                return hashCode;
            }
        }

        public static bool operator !=(SampleClass left, SampleClass right)
        {
            return !Equals(left, right);
        }

        public static bool operator ==(SampleClass left, SampleClass right)
        {
            return Equals(left, right);
        }

    }

    partial struct SampleStruct : isukces.code.AutoCode.IAutoEquatable<SampleStruct>
    {
        public override bool Equals(object other)
        {
            if (other is null) return false;
            if (other.GetType() != typeof(SampleStruct)) return false;
            return Equals((SampleStruct)other);
        }

        public bool Equals(SampleStruct other)
        {
            return StringComparer.CurrentCultureIgnoreCase.Equals(FirstName, other.FirstName)
                && StringComparer.OrdinalIgnoreCase.Equals(LastName, other.LastName)
                && DateOnlyEqualityComparer.Instance.Equals(BirthDate, other.BirthDate)
                && (OtherDate is null ? other.OtherDate is null : OtherDate.Value.Equals(other.OtherDate));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = StringComparer.CurrentCultureIgnoreCase.GetHashCode(FirstName ?? string.Empty);
                hashCode = (hashCode * 397) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(LastName ?? string.Empty);
                hashCode = (hashCode * 397) ^ DateOnlyEqualityComparer.Instance.GetHashCode(BirthDate);
                hashCode = (hashCode * 397) ^ OtherDate?.GetHashCode()  ?? 0;
                return hashCode;
            }
        }

        public static bool operator !=(SampleStruct left, SampleStruct right)
        {
            return !Equals(left, right);
        }

        public static bool operator ==(SampleStruct left, SampleStruct right)
        {
            return Equals(left, right);
        }

    }
}
