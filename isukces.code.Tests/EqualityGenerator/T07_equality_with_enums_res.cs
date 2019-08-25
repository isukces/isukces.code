using System;

// ReSharper disable once CheckNamespace
namespace isukces.code.Tests.EqualityGenerator
{
    partial class EqualityGeneratorTests
    {
        partial class ClassWithManyProperties : isukces.code.AutoCode.IAutoEquatable<EqualityGeneratorTests.ClassWithManyProperties>
        {
            public override bool Equals(object other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return other is EqualityGeneratorTests.ClassWithManyProperties otherCasted && Equals(otherCasted);
            }

            public bool Equals(EqualityGeneratorTests.ClassWithManyProperties other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return Offset == other.Offset
                    && IntValue == other.IntValue
                    && UIntValue == other.UIntValue
                    && DecimalValue.Equals(other.DecimalValue)
                    && StringComparer.Ordinal.Equals(NameNotNull, other.NameNotNull)
                    && StringComparer.OrdinalIgnoreCase.Equals(Id, other.Id)
                    && StringComparer.OrdinalIgnoreCase.Equals(IdNotNull, other.IdNotNull)
                    && ShortValue.Equals(other.ShortValue)
                    && UShortValue.Equals(other.UShortValue);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var code = (IntValue * 397 ^ UIntValue.GetHashCode()) * 397 ^ ShortValue.GetHashCode();
                    code = (code * 397 ^ UShortValue.GetHashCode()) * 397 ^ DecimalValue.GetHashCode();
                    code = code * 397 ^ StringComparer.Ordinal.GetHashCode(NameNotNull);
                    code = code * 397 ^ StringComparer.OrdinalIgnoreCase.GetHashCode(Id ?? string.Empty);
                    return (code * 397 ^ StringComparer.OrdinalIgnoreCase.GetHashCode(IdNotNull)) * 8 + (int)Offset - 5;
                }
            }

            public static bool operator !=(EqualityGeneratorTests.ClassWithManyProperties left, EqualityGeneratorTests.ClassWithManyProperties right)
            {
                return !Equals(left, right);
            }

            public static bool operator ==(EqualityGeneratorTests.ClassWithManyProperties left, EqualityGeneratorTests.ClassWithManyProperties right)
            {
                return Equals(left, right);
            }

        }

    }
}
