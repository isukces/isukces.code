#nullable disable
// ReSharper disable All
using System;
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value

// ReSharper disable once CheckNamespace
namespace EqualityGeneratorSample
{
    partial class ClassWithCompare : iSukces.Code.AutoCode.IAutoEquatable<ClassWithCompare>, iSukces.Code.AutoCode.IAutoComparable<ClassWithCompare>
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

    partial class ClassWithCustomGetHashCode : iSukces.Code.AutoCode.IAutoEquatable<ClassWithCustomGetHashCode>
    {
        public override bool Equals(object other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return other is ClassWithCustomGetHashCode otherCasted && Equals(otherCasted);
        }

        public bool Equals(ClassWithCustomGetHashCode other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return StringComparer.Ordinal.Equals(FirstName, other.FirstName)
                && StringComparer.Ordinal.Equals(LastName, other.LastName)
                && BirthDate.Equals(other.BirthDate)
                && Equals(OtherDate, other.OtherDate);
        }

        public static bool operator !=(ClassWithCustomGetHashCode left, ClassWithCustomGetHashCode right)
        {
            return !Equals(left, right);
        }

        public static bool operator ==(ClassWithCustomGetHashCode left, ClassWithCustomGetHashCode right)
        {
            return Equals(left, right);
        }

    }

    partial class ClassWithNullables : iSukces.Code.AutoCode.IAutoEquatable<ClassWithNullables>, iSukces.Code.AutoCode.IAutoComparable<ClassWithNullables>
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

    partial class ClassWithPrecomputedGetHashCode : iSukces.Code.AutoCode.IAutoEquatable<ClassWithPrecomputedGetHashCode>
    {
        public override bool Equals(object other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return other is ClassWithPrecomputedGetHashCode otherCasted && Equals(otherCasted);
        }

        public bool Equals(ClassWithPrecomputedGetHashCode other)
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
            return _cachedHashCode;
        }

        private int CalculateHashCode()
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

        public static bool operator !=(ClassWithPrecomputedGetHashCode left, ClassWithPrecomputedGetHashCode right)
        {
            return !Equals(left, right);
        }

        public static bool operator ==(ClassWithPrecomputedGetHashCode left, ClassWithPrecomputedGetHashCode right)
        {
            return Equals(left, right);
        }

        [System.Diagnostics.DebuggerBrowsableAttribute(System.Diagnostics.DebuggerBrowsableState.Never)]
        public int _cachedHashCode;

    }

    partial class ClassWithStrings : iSukces.Code.AutoCode.IAutoEquatable<ClassWithStrings>, iSukces.Code.AutoCode.IAutoComparable<ClassWithStrings>
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

    partial class ClassWithUseCachedGetHashCodeInEqualityChecking : iSukces.Code.AutoCode.IAutoEquatable<ClassWithUseCachedGetHashCodeInEqualityChecking>
    {
        public override bool Equals(object other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return other is ClassWithUseCachedGetHashCodeInEqualityChecking otherCasted && Equals(otherCasted);
        }

        public bool Equals(ClassWithUseCachedGetHashCodeInEqualityChecking other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetHashCode() != other.GetHashCode()) return false;
            return StringComparer.Ordinal.Equals(FirstName, other.FirstName)
                && StringComparer.Ordinal.Equals(LastName, other.LastName)
                && BirthDate.Equals(other.BirthDate)
                && Equals(OtherDate, other.OtherDate);
        }

        public override int GetHashCode()
        {
            if (_isCachedHashCodeCalculated) return _cachedHashCode;
            _cachedHashCode = CalculateHashCode();
            _isCachedHashCodeCalculated = true;
            return _cachedHashCode;
        }

        private int CalculateHashCode()
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

        public static bool operator !=(ClassWithUseCachedGetHashCodeInEqualityChecking left, ClassWithUseCachedGetHashCodeInEqualityChecking right)
        {
            return !Equals(left, right);
        }

        public static bool operator ==(ClassWithUseCachedGetHashCodeInEqualityChecking left, ClassWithUseCachedGetHashCodeInEqualityChecking right)
        {
            return Equals(left, right);
        }

        [System.Diagnostics.DebuggerBrowsableAttribute(System.Diagnostics.DebuggerBrowsableState.Never)]
        public bool _isCachedHashCodeCalculated;

        [System.Diagnostics.DebuggerBrowsableAttribute(System.Diagnostics.DebuggerBrowsableState.Never)]
        public int _cachedHashCode;

    }

    partial class ClassWithUsePrecomputedGetHashCodeInEqualityChecking : iSukces.Code.AutoCode.IAutoEquatable<ClassWithUsePrecomputedGetHashCodeInEqualityChecking>
    {
        public override bool Equals(object other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return other is ClassWithUsePrecomputedGetHashCodeInEqualityChecking otherCasted && Equals(otherCasted);
        }

        public bool Equals(ClassWithUsePrecomputedGetHashCodeInEqualityChecking other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (_cachedHashCode != other._cachedHashCode) return false;
            return StringComparer.Ordinal.Equals(FirstName, other.FirstName)
                && StringComparer.Ordinal.Equals(LastName, other.LastName)
                && BirthDate.Equals(other.BirthDate)
                && Equals(OtherDate, other.OtherDate);
        }

        public override int GetHashCode()
        {
            return _cachedHashCode;
        }

        private int CalculateHashCode()
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

        public static bool operator !=(ClassWithUsePrecomputedGetHashCodeInEqualityChecking left, ClassWithUsePrecomputedGetHashCodeInEqualityChecking right)
        {
            return !Equals(left, right);
        }

        public static bool operator ==(ClassWithUsePrecomputedGetHashCodeInEqualityChecking left, ClassWithUsePrecomputedGetHashCodeInEqualityChecking right)
        {
            return Equals(left, right);
        }

        [System.Diagnostics.DebuggerBrowsableAttribute(System.Diagnostics.DebuggerBrowsableState.Never)]
        public int _cachedHashCode;

    }

    partial class SimpleClass : iSukces.Code.AutoCode.IAutoEquatable<SimpleClass>
    {
        public override bool Equals(object other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return other is SimpleClass otherCasted && Equals(otherCasted);
        }

        public bool Equals(SimpleClass other)
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

        public static bool operator !=(SimpleClass left, SimpleClass right)
        {
            return !Equals(left, right);
        }

        public static bool operator ==(SimpleClass left, SimpleClass right)
        {
            return Equals(left, right);
        }

    }

    partial struct SimpleStruct : iSukces.Code.AutoCode.IAutoEquatable<SimpleStruct>
    {
        public override bool Equals(object other)
        {
            if (other is null) return false;
            if (other.GetType() != typeof(SimpleStruct)) return false;
            return Equals((SimpleStruct)other);
        }

        public bool Equals(SimpleStruct other)
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

        public static bool operator !=(SimpleStruct left, SimpleStruct right)
        {
            return !Equals(left, right);
        }

        public static bool operator ==(SimpleStruct left, SimpleStruct right)
        {
            return Equals(left, right);
        }

    }

    partial class VerySimpleClassOwner : iSukces.Code.AutoCode.IAutoEquatable<VerySimpleClassOwner>
    {
        public override bool Equals(object other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return other is VerySimpleClassOwner otherCasted && Equals(otherCasted);
        }

        public bool Equals(VerySimpleClassOwner other)
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

        public static bool operator !=(VerySimpleClassOwner left, VerySimpleClassOwner right)
        {
            return !Equals(left, right);
        }

        public static bool operator ==(VerySimpleClassOwner left, VerySimpleClassOwner right)
        {
            return Equals(left, right);
        }

    }
}

