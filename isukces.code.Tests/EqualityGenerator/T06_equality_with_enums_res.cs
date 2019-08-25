using System;

// ReSharper disable once CheckNamespace
namespace isukces.code.Tests.EqualityGenerator
{
    partial class EqualityGeneratorTests
    {
        partial class ClassWithEnumProperties4 : isukces.code.AutoCode.IAutoEquatable<EqualityGeneratorTests.ClassWithEnumProperties4>
        {
            public override bool Equals(object other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return other is ClassWithEnumProperties4 otherCasted && Equals(otherCasted);
            }

            public bool Equals(ClassWithEnumProperties4 other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return Offset == other.Offset
                    && IntValue == other.IntValue;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return IntValue * 8 + (int)Offset - 5;
                }
            }

            public static bool operator !=(ClassWithEnumProperties4 left, ClassWithEnumProperties4 right)
            {
                return !Equals(left, right);
            }

            public static bool operator ==(ClassWithEnumProperties4 left, ClassWithEnumProperties4 right)
            {
                return Equals(left, right);
            }

        }

    }
}
