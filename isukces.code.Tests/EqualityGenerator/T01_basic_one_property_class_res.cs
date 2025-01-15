// ReSharper disable All
using System;

// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace iSukces.Code.Tests.EqualityGenerator
{
    partial class EqualityGeneratorTests
    {
        partial class OnePropertyClass : iSukces.Code.AutoCode.IAutoEquatable<EqualityGeneratorTests.OnePropertyClass>
        {
            public override bool Equals(object? other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return other is OnePropertyClass otherCasted && Equals(otherCasted);
            }

            public bool Equals(OnePropertyClass? other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return IntValue == other.IntValue;
            }

            public override int GetHashCode()
            {
                return IntValue;
            }

            public static bool operator ==(OnePropertyClass left, OnePropertyClass right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(OnePropertyClass left, OnePropertyClass right)
            {
                return !Equals(left, right);
            }

        }

    }
}
