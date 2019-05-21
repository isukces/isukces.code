using System;
using isukces.code.interfaces;

namespace EqualityGeneratorSample
{
    [Auto.EqualityGeneratorAttribute(null,
        UseGetHashCodeInEqualityChecking = true,
        CachedGetHashCodeImplementation  = GetHashCodeImplementationKind.Precomputed)]
    partial class ClassWithUsePrecomputedGetHashCodeInEqualityChecking
    {
        public string    FirstName { get; }
        public string    LastName  { get; }
        public DateTime  BirthDate { get; }
        public DateTime? OtherDate { get; }
    }
}