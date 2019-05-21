using System;
using isukces.code.interfaces;

namespace EqualityGeneratorSample
{
    [Auto.EqualityGeneratorAttribute(null, CachedGetHashCodeImplementation = GetHashCodeImplementationKind.Precomputed)]
    public partial class ClassWithPrecomputedGetHashCode
    {
        public ClassWithPrecomputedGetHashCode(string firstName, string lastName, DateTime birthDate,
            DateTime? otherDate)
        {
            FirstName       = firstName;
            LastName        = lastName;
            BirthDate       = birthDate;
            OtherDate       = otherDate;
            _cachedHashCode = CalculateHashCode();
        }

        public string    FirstName { get; }
        public string    LastName  { get; }
        public DateTime  BirthDate { get; }
        public DateTime? OtherDate { get; }
    }
}