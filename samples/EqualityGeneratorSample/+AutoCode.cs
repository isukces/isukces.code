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
            var comparer1 = StringComparer.OrdinalIgnoreCase;
            var lastNameComparison = comparer1.Compare(LastName, other.LastName);
            if (lastNameComparison != 0) return lastNameComparison;
            var comparer2 = StringComparer.CurrentCultureIgnoreCase;
            var firstNameComparison = comparer2.Compare(FirstName, other.FirstName);
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

    partial class ClassWithNullables : isukces.code.AutoCode.IAutoEquatable<ClassWithNullables>, isukces.code.AutoCode.IAutoComparable<ClassWithNullables>
    {
        public int CompareTo(ClassWithNullables other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (other is null) return 1;
            var comparer1 = System.Collections.Generic.Comparer<int?>.Default;
            var normalComparison = comparer1.Compare(Normal, other.Normal);
            if (normalComparison != 0) return normalComparison;
            var nullAreEqualComparison = comparer1.Compare(NullAreEqual, other.NullAreEqual);
            if (nullAreEqualComparison != 0) return nullAreEqualComparison;
            var comparer2 = System.Collections.Generic.Comparer<int>.Default;
            var nullLikeEmptyComparison = comparer2.Compare(NullLikeEmpty ?? 0, other.NullLikeEmpty ?? 0);
            if (nullLikeEmptyComparison != 0) return nullLikeEmptyComparison;
            return comparer2.Compare(Both ?? 0, other.Both ?? 0);
        }

        public int CompareTo(object obj)
        {
            if (obj is null) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            return obj is ClassWithNullables other ? CompareTo(other) : throw new ArgumentException("Object must be of type ClassWithNullables");
        }

        public override bool Equals(object other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return other is ClassWithNullables otherCasted && Equals(otherCasted);
        }

        public bool Equals(ClassWithNullables other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Normal, other.Normal)
                && (ReferenceEquals(NullAreEqual, null) && ReferenceEquals(other.NullAreEqual, null) || Equals(NullAreEqual, other.NullAreEqual))
                && (NullLikeEmpty ?? 0).Equals(other.NullLikeEmpty ?? 0)
                && (ReferenceEquals(Both ?? 0, null) && ReferenceEquals(other.Both ?? 0, null) || (Both ?? 0).Equals(other.Both ?? 0));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Normal?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ NullAreEqual?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (NullLikeEmpty ?? 0).GetHashCode();
                hashCode = (hashCode * 397) ^ (Both ?? 0).GetHashCode();
                return hashCode;
            }
        }

        public static bool operator !=(ClassWithNullables left, ClassWithNullables right)
        {
            return !Equals(left, right);
        }

        public static bool operator <(ClassWithNullables left, ClassWithNullables right)
        {
            if (Equals(left, right)) return false;
            if (left is null) // null.CompareTo(NOTNULL) = -1
                return true;
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(ClassWithNullables left, ClassWithNullables right)
        {
            if (Equals(left, right)) return true;
            if (left is null) // null.CompareTo(NOTNULL) = -1
                return true;
            return left.CompareTo(right) <= 0;
        }

        public static bool operator ==(ClassWithNullables left, ClassWithNullables right)
        {
            return Equals(left, right);
        }

        public static bool operator >(ClassWithNullables left, ClassWithNullables right)
        {
            if (Equals(left, right)) return false;
            if (left is null) // null.CompareTo(NOTNULL) = -1
                return false;
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(ClassWithNullables left, ClassWithNullables right)
        {
            if (Equals(left, right)) return true;
            if (left is null) // null.CompareTo(NOTNULL) = -1
                return false;
            return left.CompareTo(right) >= 0;
        }

    }

    partial class ClassWithStrings : isukces.code.AutoCode.IAutoEquatable<ClassWithStrings>, isukces.code.AutoCode.IAutoComparable<ClassWithStrings>
    {
        public int CompareTo(ClassWithStrings other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (other is null) return 1;
            var normalComparison = StringComparer.Ordinal.Compare(Normal, other.Normal);
            if (normalComparison != 0) return normalComparison;
            var nullAreEqualComparison = StringComparer.Ordinal.Compare(NullAreEqual, other.NullAreEqual);
            if (nullAreEqualComparison != 0) return nullAreEqualComparison;
            var nullLikeEmptyComparison = StringComparer.Ordinal.Compare(NullLikeEmpty ?? string.Empty, other.NullLikeEmpty ?? string.Empty);
            if (nullLikeEmptyComparison != 0) return nullLikeEmptyComparison;
            return StringComparer.Ordinal.Compare(Both ?? string.Empty, other.Both ?? string.Empty);
        }

        public int CompareTo(object obj)
        {
            if (obj is null) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            return obj is ClassWithStrings other ? CompareTo(other) : throw new ArgumentException("Object must be of type ClassWithStrings");
        }

        public override bool Equals(object other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return other is ClassWithStrings otherCasted && Equals(otherCasted);
        }

        public bool Equals(ClassWithStrings other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return StringComparer.Ordinal.Equals(Normal, other.Normal)
                && (ReferenceEquals(NullAreEqual, null) && ReferenceEquals(other.NullAreEqual, null) || StringComparer.Ordinal.Equals(NullAreEqual, other.NullAreEqual))
                && StringComparer.Ordinal.Equals(NullLikeEmpty ?? string.Empty, other.NullLikeEmpty ?? string.Empty)
                && (string.IsNullOrEmpty(Both) && string.IsNullOrEmpty(other.Both) || StringComparer.Ordinal.Equals(Both ?? string.Empty, other.Both ?? string.Empty));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = StringComparer.Ordinal.GetHashCode(Normal ?? string.Empty);
                hashCode = (hashCode * 397) ^ StringComparer.Ordinal.GetHashCode(NullAreEqual ?? string.Empty);
                hashCode = (hashCode * 397) ^ StringComparer.Ordinal.GetHashCode(NullLikeEmpty ?? string.Empty);
                hashCode = (hashCode * 397) ^ StringComparer.Ordinal.GetHashCode(Both ?? string.Empty);
                return hashCode;
            }
        }

        public static bool operator !=(ClassWithStrings left, ClassWithStrings right)
        {
            return !Equals(left, right);
        }

        public static bool operator <(ClassWithStrings left, ClassWithStrings right)
        {
            if (Equals(left, right)) return false;
            if (left is null) // null.CompareTo(NOTNULL) = -1
                return true;
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(ClassWithStrings left, ClassWithStrings right)
        {
            if (Equals(left, right)) return true;
            if (left is null) // null.CompareTo(NOTNULL) = -1
                return true;
            return left.CompareTo(right) <= 0;
        }

        public static bool operator ==(ClassWithStrings left, ClassWithStrings right)
        {
            return Equals(left, right);
        }

        public static bool operator >(ClassWithStrings left, ClassWithStrings right)
        {
            if (Equals(left, right)) return false;
            if (left is null) // null.CompareTo(NOTNULL) = -1
                return false;
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(ClassWithStrings left, ClassWithStrings right)
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
                && Equals(OtherDate, other.OtherDate);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = StringComparer.Ordinal.GetHashCode(FirstName ?? string.Empty);
                hashCode = (hashCode * 397) ^ StringComparer.Ordinal.GetHashCode(LastName ?? string.Empty);
                hashCode = (hashCode * 397) ^ BirthDate.GetHashCode();
                hashCode = (hashCode * 397) ^ OtherDate?.GetHashCode() ?? 0;
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
                && Equals(OtherDate, other.OtherDate);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = StringComparer.CurrentCultureIgnoreCase.GetHashCode(FirstName ?? string.Empty);
                hashCode = (hashCode * 397) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(LastName ?? string.Empty);
                hashCode = (hashCode * 397) ^ DateOnlyComparer.GetHashCode(BirthDate);
                hashCode = (hashCode * 397) ^ OtherDate?.GetHashCode() ?? 0;
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
