using System;

// ReSharper disable once CheckNamespace
namespace isukces.code.Tests.EqualityGenerator
{
    partial class EqualityGeneratorTests
    {
        partial class OnePropertyClass : isukces.code.AutoCode.IAutoEquatable<EqualityGeneratorTests.OnePropertyClass>
        {
            public override bool Equals(object other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return other is EqualityGeneratorTests.OnePropertyClass otherCasted && Equals(otherCasted);
            }

            public bool Equals(EqualityGeneratorTests.OnePropertyClass other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return IntValue == other.IntValue;
            }

            public override int GetHashCode()
            {
                return IntValue;
            }

            public static bool operator !=(EqualityGeneratorTests.OnePropertyClass left, EqualityGeneratorTests.OnePropertyClass right)
            {
                return !Equals(left, right);
            }

            public static bool operator ==(EqualityGeneratorTests.OnePropertyClass left, EqualityGeneratorTests.OnePropertyClass right)
            {
                return Equals(left, right);
            }

        }

    }
}
