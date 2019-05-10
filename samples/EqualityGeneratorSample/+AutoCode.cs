// ReSharper disable All
using System;

// ReSharper disable once CheckNamespace
namespace EqualityGeneratorSample
{
    partial class ClassWithCompare : isukces.code.AutoCode.IAutoEquatable<ClassWithCompare>, isukces.code.AutoCode.IAutoComparable<ClassWithCompare>
    {
        public int CompareTo(ClassWithCompare other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (other is null) return 1;
            var lastNameComparison = StringComparer.OrdinalIgnoreCase.Compare(LastName, other.LastName);
            if (lastNameComparison != 0) return lastNameComparison;
            var firstNameComparison = StringComparer.CurrentCultureIgnoreCase.Compare(FirstName, other.FirstName);
            if (firstNameComparison != 0) return firstNameComparison;
            var birthDateComparison = DateOnlyComparer.CompareNullable(BirthDate, other.BirthDate);
            if (birthDateComparison != 0) return birthDateComparison;
            return StringComparer.Ordinal.Compare(Code, other.Code);
        }

        public int CompareTo(object obj)
        {
            if (obj is null) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            return obj is ClassWithCompare other ? CompareTo(other) : throw new ArgumentException("Object must be of type ClassWithCompare");
        }

        public override bool Equals(object other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return other is ClassWithCompare otherCasted && Equals(otherCasted);
        }

        public bool Equals(ClassWithCompare other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return StringComparer.CurrentCultureIgnoreCase.Equals(FirstName, other.FirstName)
                && StringComparer.OrdinalIgnoreCase.Equals(LastName, other.LastName)
                && StringComparer.Ordinal.Equals(Code, other.Code)
                && DateOnlyComparer.EqualsNullable(BirthDate, other.BirthDate);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = StringComparer.CurrentCultureIgnoreCase.GetHashCode(FirstName ?? string.Empty);
                hashCode = (hashCode * 397) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(LastName ?? string.Empty);
                hashCode = (hashCode * 397) ^ StringComparer.Ordinal.GetHashCode(Code ?? string.Empty);
                hashCode = (hashCode * 397) ^ (BirthDate is null ? 0 : DateOnlyComparer.GetHashCode(BirthDate.Value));
                return hashCode;
            }
        }

        public static bool operator !=(ClassWithCompare left, ClassWithCompare right)
        {
            return !Equals(left, right);
        }

        public static bool operator <(ClassWithCompare left, ClassWithCompare right)
        {
            if (Equals(left, right)) return false;
            if (left is null) // null.CompareTo(NOTNULL) = -1
                return true;
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(ClassWithCompare left, ClassWithCompare right)
        {
            if (Equals(left, right)) return true;
            if (left is null) // null.CompareTo(NOTNULL) = -1
                return true;
            return left.CompareTo(right) <= 0;
        }

        public static bool operator ==(ClassWithCompare left, ClassWithCompare right)
        {
            return Equals(left, right);
        }

        public static bool operator >(ClassWithCompare left, ClassWithCompare right)
        {
            if (Equals(left, right)) return false;
            if (left is null) // null.CompareTo(NOTNULL) = -1
                return false;
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(ClassWithCompare left, ClassWithCompare right)
        {
            if (Equals(left, right)) return true;
            if (left is null) // null.CompareTo(NOTNULL) = -1
                return false;
            return left.CompareTo(right) >= 0;
        }

    }

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
            return StringComparer.Ordinal.Equals(FirstName, other.FirstName)
                && StringComparer.Ordinal.Equals(LastName, other.LastName)
                && BirthDate.Equals(other.BirthDate)
                && (OtherDate is null ? other.OtherDate is null : OtherDate.Value.Equals(other.OtherDate));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = StringComparer.Ordinal.GetHashCode(FirstName ?? string.Empty);
                hashCode = (hashCode * 397) ^ StringComparer.Ordinal.GetHashCode(LastName ?? string.Empty);
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
                && DateOnlyComparer.Equals(BirthDate, other.BirthDate)
                && (OtherDate is null ? other.OtherDate is null : OtherDate.Value.Equals(other.OtherDate));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = StringComparer.CurrentCultureIgnoreCase.GetHashCode(FirstName ?? string.Empty);
                hashCode = (hashCode * 397) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(LastName ?? string.Empty);
                hashCode = (hashCode * 397) ^ DateOnlyComparer.GetHashCode(BirthDate);
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
