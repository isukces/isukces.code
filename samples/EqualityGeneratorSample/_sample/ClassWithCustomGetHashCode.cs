#nullable disable
using System;
using iSukces.Code.Interfaces;

namespace EqualityGeneratorSample
{
    [Auto.EqualityGeneratorAttribute(null, CachedGetHashCodeImplementation = GetHashCodeImplementationKind.Custom)]
    public partial class ClassWithCustomGetHashCode
    {
        public ClassWithCustomGetHashCode(string firstName, string lastName, DateTime birthDate,
            DateTime? otherDate)
        {
            FirstName = firstName;
            LastName  = lastName;
            BirthDate = birthDate;
            OtherDate = otherDate;
            // _cachedHashCode = CalculateHashCode();
        }

        public override int GetHashCode()
        {
            // make this code manually
            unchecked
            {
                var hashCode = StringComparer.Ordinal.GetHashCode(FirstName ?? string.Empty);
                hashCode = (hashCode * 397) ^ StringComparer.Ordinal.GetHashCode(LastName ?? string.Empty);
                hashCode = (hashCode * 397) ^ BirthDate.GetHashCode();
                hashCode = (hashCode * 397) ^ OtherDate?.GetHashCode() ?? 0;
                return hashCode;
            }
        }

        public string    FirstName { get; }
        public string    LastName  { get; }
        public DateTime  BirthDate { get; }
        public DateTime? OtherDate { get; }
    }
}
