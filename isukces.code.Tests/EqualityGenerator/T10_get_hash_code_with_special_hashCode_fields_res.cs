// ReSharper disable All
using System;

namespace iSukces.Code.Tests.EqualityGenerator
{
    partial struct TestStructWithSpecialHashCodeField : iSukces.Code.AutoCode.IAutoEquatable<TestStructWithSpecialHashCodeField>
    {
        public override bool Equals(object other)
        {
            if (other is null) return false;
            if (other.GetType() != typeof(TestStructWithSpecialHashCodeField)) return false;
            return Equals((TestStructWithSpecialHashCodeField)other);
        }

        public bool Equals(TestStructWithSpecialHashCodeField other)
        {
            if (IsEmpty) return other.IsEmpty;
            if (other.IsEmpty) return false;
            return StringComparer.OrdinalIgnoreCase.Equals(Name, other.Name);
        }

        public override int GetHashCode()
        {
            return IsEmpty ? 0 : HashCode;
        }

        public static bool operator !=(TestStructWithSpecialHashCodeField left, TestStructWithSpecialHashCodeField right)
        {
            return !Equals(left, right);
        }

        public static bool operator ==(TestStructWithSpecialHashCodeField left, TestStructWithSpecialHashCodeField right)
        {
            return Equals(left, right);
        }

    }
}
