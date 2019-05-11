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
            var comparer = StringComparer.OrdinalIgnoreCase;
            var comparisonResult = comparer.Compare(LastName, other.LastName);
            if (comparisonResult != 0) return comparisonResult;
            var comparer2 = StringComparer.CurrentCultureIgnoreCase;
            comparisonResult = comparer2.Compare(FirstName, other.FirstName);
            if (comparisonResult != 0) return comparisonResult;
            comparisonResult = DateOnlyComparer.CompareNullable(BirthDate, other.BirthDate);
            if (comparisonResult != 0) return comparisonResult;
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
            var comparer = System.Collections.Generic.Comparer<int?>.Default;
            var comparisonResult = comparer.Compare(Normal, other.Normal);
            if (comparisonResult != 0) return comparisonResult;
            return (NullLikeEmpty ?? 0).CompareTo(other.NullLikeEmpty ?? 0);
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
                && (NullLikeEmpty ?? 0).Equals(other.NullLikeEmpty ?? 0);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Normal ?? 0 * 397) ^ NullLikeEmpty ?? 0;
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
            var comparisonResult = StringComparer.Ordinal.Compare(Normal, other.Normal);
            if (comparisonResult != 0) return comparisonResult;
            return StringComparer.Ordinal.Compare(NullLikeEmpty ?? string.Empty, other.NullLikeEmpty ?? string.Empty);
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
                && StringComparer.Ordinal.Equals(NullLikeEmpty ?? string.Empty, other.NullLikeEmpty ?? string.Empty);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (StringComparer.Ordinal.GetHashCode(Normal ?? string.Empty) * 397) ^ StringComparer.Ordinal.GetHashCode(NullLikeEmpty ?? string.Empty);
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

    partial class Sample1Owner : isukces.code.AutoCode.IAutoEquatable<Sample1Owner>
    {
        public override bool Equals(object other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return other is Sample1Owner otherCasted && Equals(otherCasted);
        }

        public bool Equals(Sample1Owner other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(A, other.A)
                && Equals(B, other.B)
                && ANotNull.Equals(other.ANotNull)
                && BNotNull.Equals(other.BNotNull);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = A?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ B?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ ANotNull.GetHashCode();
                hashCode = (hashCode * 397) ^ BNotNull.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator !=(Sample1Owner left, Sample1Owner right)
        {
            return !Equals(left, right);
        }

        public static bool operator ==(Sample1Owner left, Sample1Owner right)
        {
            return Equals(left, right);
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
