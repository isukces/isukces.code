using System;
using isukces.code.interfaces;

namespace EqualityGeneratorSample
{
    [Auto.EqualityGeneratorAttribute(null, nameof(FullName))]
    [Auto.ComparerGeneratorAttribute(nameof(LastName), nameof(FirstName), nameof(BirthDate), nameof(Code))]
    public partial class ClassWithCompare
    {
        [Auto.StringComparison.CurrentCultureIgnoreCaseAttribute]
        public string FirstName { get; set; }

        [Auto.StringComparison.OrdinalIgnoreCaseAttribute]
        public string LastName { get; set; }

        public string Code { get; set; }

        public string FullName => FirstName + " " + LastName;


        [DateOnlyEquality]
        public DateTime? BirthDate { get; set; }
    }
}