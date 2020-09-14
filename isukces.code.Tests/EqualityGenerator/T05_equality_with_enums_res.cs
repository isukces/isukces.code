// ReSharper disable All
using System;

namespace iSukces.Code.Tests.EqualityGenerator
{
    partial class EqualityGeneratorTests
    {
        partial class ClassWithEnumProperties3 : iSukces.Code.AutoCode.IAutoEquatable<EqualityGeneratorTests.ClassWithEnumProperties3>
        {
            public override bool Equals(object other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return other is ClassWithEnumProperties3 otherCasted && Equals(otherCasted);
            }

            public bool Equals(ClassWithEnumProperties3 other)
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

            public static bool operator !=(ClassWithEnumProperties3 left, ClassWithEnumProperties3 right)
            {
                return !Equals(left, right);
            }

            public static bool operator ==(ClassWithEnumProperties3 left, ClassWithEnumProperties3 right)
            {
                return Equals(left, right);
            }

        }

    }
}
