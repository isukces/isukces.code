// ReSharper disable All
using System;

namespace isukces.code.Tests.EqualityGenerator
{
    partial class EqualityGeneratorTests
    {
        partial struct OnePropertyStruct : isukces.code.AutoCode.IAutoEquatable<EqualityGeneratorTests.OnePropertyStruct>
        {
            public override bool Equals(object other)
            {
                if (other is null) return false;
                if (other.GetType() != typeof(OnePropertyStruct)) return false;
                return Equals((OnePropertyStruct)other);
            }

            public bool Equals(OnePropertyStruct other)
            {
                return IntValue == other.IntValue;
            }

            public override int GetHashCode()
            {
                return IntValue;
            }

            public static bool operator !=(OnePropertyStruct left, OnePropertyStruct right)
            {
                return !Equals(left, right);
            }

            public static bool operator ==(OnePropertyStruct left, OnePropertyStruct right)
            {
                return Equals(left, right);
            }

        }

    }
}
