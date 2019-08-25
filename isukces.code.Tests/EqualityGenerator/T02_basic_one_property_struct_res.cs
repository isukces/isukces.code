using System;

// ReSharper disable once CheckNamespace
namespace isukces.code.Tests.EqualityGenerator
{
    partial class EqualityGeneratorTests
    {
        partial struct OnePropertyStruct : isukces.code.AutoCode.IAutoEquatable<EqualityGeneratorTests.OnePropertyStruct>
        {
            public override bool Equals(object other)
            {
                if (other is null) return false;
                if (other.GetType() != typeof(EqualityGeneratorTests.OnePropertyStruct)) return false;
                return Equals((EqualityGeneratorTests.OnePropertyStruct)other);
            }

            public bool Equals(EqualityGeneratorTests.OnePropertyStruct other)
            {
                return IntValue == other.IntValue;
            }

            public override int GetHashCode()
            {
                return IntValue;
            }

            public static bool operator !=(EqualityGeneratorTests.OnePropertyStruct left, EqualityGeneratorTests.OnePropertyStruct right)
            {
                return !Equals(left, right);
            }

            public static bool operator ==(EqualityGeneratorTests.OnePropertyStruct left, EqualityGeneratorTests.OnePropertyStruct right)
            {
                return Equals(left, right);
            }

        }

    }
}
