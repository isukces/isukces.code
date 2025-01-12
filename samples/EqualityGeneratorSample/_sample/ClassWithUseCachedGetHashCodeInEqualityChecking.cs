#nullable disable
using System;
using iSukces.Code.Interfaces;

namespace EqualityGeneratorSample
{
    [Auto.EqualityGeneratorAttribute(null,
        UseGetHashCodeInEqualityChecking = true,
        CachedGetHashCodeImplementation  = GetHashCodeImplementationKind.Cached)]
    partial class ClassWithUseCachedGetHashCodeInEqualityChecking
    {
        public string    FirstName { get; }
        public string    LastName  { get; }
        public DateTime  BirthDate { get; }
        public DateTime? OtherDate { get; }
    }
}
