// ReSharper disable All
using System;

// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace iSukces.Code.Tests.EqualityGenerator
{
    partial class EqualityGeneratorTests
    {
        partial class ClassWithEnumProperties2 : iSukces.Code.AutoCode.IAutoEquatable<EqualityGeneratorTests.ClassWithEnumProperties2>
        {
            public override bool Equals(object other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return other is ClassWithEnumProperties2 otherCasted && Equals(otherCasted);
            }

            public bool Equals(ClassWithEnumProperties2 other)
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

            public static bool operator ==(ClassWithEnumProperties2 left, ClassWithEnumProperties2 right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(ClassWithEnumProperties2 left, ClassWithEnumProperties2 right)
            {
                return !Equals(left, right);
            }

        }

    }
}
