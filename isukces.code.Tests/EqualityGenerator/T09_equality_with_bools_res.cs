// ReSharper disable All
using System;

// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace iSukces.Code.Tests.EqualityGenerator
{
    partial class EqualityGeneratorTests
    {
        partial class ClassWithBools : iSukces.Code.AutoCode.IAutoEquatable<EqualityGeneratorTests.ClassWithBools>
        {
            public override bool Equals(object? other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return other is ClassWithBools otherCasted && Equals(otherCasted);
            }

            public bool Equals(ClassWithBools? other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return BoolNormal == other.BoolNormal
                    && Value1 == other.Value1
                    && EnumNullable == other.EnumNullable
                    && Value2 == other.Value2
                    && Value3 == other.Value3
                    && Value4 == other.Value4
                    && Equals(BoolNullable, other.BoolNullable);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var code = (((Value1 * 397 ^ Value2) * 397 ^ Value3) * 397 ^ Value4) * 8 + (int)EnumNullable - 5;
                    return (code * 2 + (BoolNormal ? 1 : 0)) * 2 + (BoolNullable == true ? 1 : 0);
                }
            }

            public static bool operator ==(ClassWithBools left, ClassWithBools right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(ClassWithBools left, ClassWithBools right)
            {
                return !Equals(left, right);
            }

        }

    }
}
