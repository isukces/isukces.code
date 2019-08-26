// ReSharper disable All
using System;

namespace isukces.code.Tests.EqualityGenerator
{
    partial class EqualityGeneratorTests
    {
        partial class ClassWithEnumProperties1 : isukces.code.AutoCode.IAutoEquatable<EqualityGeneratorTests.ClassWithEnumProperties1>
        {
            public override bool Equals(object other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return other is ClassWithEnumProperties1 otherCasted && Equals(otherCasted);
            }

            public bool Equals(ClassWithEnumProperties1 other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return N1 == other.N1
                    && O1 == other.O1
                    && N2 == other.N2
                    && O2 == other.O2;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (((int)N1 * 8 + (int)O1 - 5) * 3 + (int)N2) * 8 + (int)O2 - 5;
                }
            }

            public static bool operator !=(ClassWithEnumProperties1 left, ClassWithEnumProperties1 right)
            {
                return !Equals(left, right);
            }

            public static bool operator ==(ClassWithEnumProperties1 left, ClassWithEnumProperties1 right)
            {
                return Equals(left, right);
            }

        }

    }
}
