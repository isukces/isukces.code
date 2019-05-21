using System;
using isukces.code.interfaces;

namespace EqualityGeneratorSample
{
    [Auto.EqualityGeneratorAttribute]
    public partial struct SimpleStruct
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