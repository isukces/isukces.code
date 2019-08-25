using System;

// ReSharper disable once CheckNamespace
namespace isukces.code.Tests.EqualityGenerator
{
    partial class EqualityGeneratorTests
    {
        partial class ClassWithEnumProperties2 : isukces.code.AutoCode.IAutoEquatable<EqualityGeneratorTests.ClassWithEnumProperties2>
        {
            public override bool Equals(object other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return other is EqualityGeneratorTests.ClassWithEnumProperties2 otherCasted && Equals(otherCasted);
            }

            public bool Equals(EqualityGeneratorTests.ClassWithEnumProperties2 other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return IntValue == other.IntValue
                    && Normal == other.Normal;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return IntValue * 3 + (int)Normal;
                }
            }

            public static bool operator !=(EqualityGeneratorTests.ClassWithEnumProperties2 left, EqualityGeneratorTests.ClassWithEnumProperties2 right)
            {
                return !Equals(left, right);
            }

            public static bool operator ==(EqualityGeneratorTests.ClassWithEnumProperties2 left, EqualityGeneratorTests.ClassWithEnumProperties2 right)
            {
                return Equals(left, right);
            }

        }

    }
}
