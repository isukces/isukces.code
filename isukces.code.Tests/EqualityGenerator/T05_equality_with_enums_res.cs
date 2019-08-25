using System;

// ReSharper disable once CheckNamespace
namespace isukces.code.Tests.EqualityGenerator
{
    partial class EqualityGeneratorTests
    {
        partial class ClassWithEnumProperties3 : isukces.code.AutoCode.IAutoEquatable<EqualityGeneratorTests.ClassWithEnumProperties3>
        {
            public override bool Equals(object other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return other is EqualityGeneratorTests.ClassWithEnumProperties3 otherCasted && Equals(otherCasted);
            }

            public bool Equals(EqualityGeneratorTests.ClassWithEnumProperties3 other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return IntValue == other.IntValue
                    && Offset == other.Offset;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return IntValue * 8 + (int)Offset - 5;
                }
            }

            public static bool operator !=(EqualityGeneratorTests.ClassWithEnumProperties3 left, EqualityGeneratorTests.ClassWithEnumProperties3 right)
            {
                return !Equals(left, right);
            }

            public static bool operator ==(EqualityGeneratorTests.ClassWithEnumProperties3 left, EqualityGeneratorTests.ClassWithEnumProperties3 right)
            {
                return Equals(left, right);
            }

        }

    }
}
