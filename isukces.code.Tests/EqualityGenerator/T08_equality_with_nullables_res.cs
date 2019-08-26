// ReSharper disable All
using System;

namespace isukces.code.Tests.EqualityGenerator
{
    partial class EqualityGeneratorTests
    {
        partial class ClassWithNullable : isukces.code.AutoCode.IAutoEquatable<EqualityGeneratorTests.ClassWithNullable>
        {
            public override bool Equals(object other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return other is ClassWithNullable otherCasted && Equals(otherCasted);
            }

            public bool Equals(ClassWithNullable other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return Equals(EnumNullable, other.EnumNullable)
                    && Equals(IntNullable, other.IntNullable);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (EnumNullable?.GetHashCode() ?? 0) * 397 ^ (IntNullable ?? 0);
                }
            }

            public static bool operator !=(ClassWithNullable left, ClassWithNullable right)
            {
                return !Equals(left, right);
            }

            public static bool operator ==(ClassWithNullable left, ClassWithNullable right)
            {
                return Equals(left, right);
            }

        }

    }
}
