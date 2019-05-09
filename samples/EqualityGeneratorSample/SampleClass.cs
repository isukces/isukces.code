using System;
using isukces.code.interfaces;

namespace EqualityGeneratorSample
{
    [Auto.EqualityGeneratorAttribute]
    public partial class SampleClass
    {
        public string    FirstName { get; set; }
        public string    LastName  { get; set; }
        public DateTime  BirthDate { get; set; }
        public DateTime? OtherDate { get; set; }
    }

    [Auto.EqualityGeneratorAttribute]
    public partial struct SampleStruct
    {
        [Auto.StringComparison.CurrentCultureIgnoreCaseAttribute]
        public string FirstName { get; set; }

        [Auto.StringComparison.OrdinalIgnoreCaseAttribute]
        public string LastName { get; set; }

        [DateOnlyEquality]
        public DateTime BirthDate { get; set; }

        public DateTime? OtherDate { get; set; }
    }
}