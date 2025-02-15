#nullable disable
using System;
using iSukces.Code.Interfaces;

namespace EqualityGeneratorSample
{
    [Auto.EqualityGeneratorAttribute]
    [Auto.ComparerGeneratorAttribute(nameof(LastName), nameof(FirstName), nameof(BirthDate), nameof(Code))]
    public partial class ClassWithCompare
    {
        [Auto.StringComparison.CurrentCultureIgnoreCase]
        public string FirstName { get; set; }

        [Auto.StringComparison.OrdinalIgnoreCaseAttribute]
        public string LastName { get; set; }

        public string Code { get; set; }

        [Auto.EqualityGeneratorSkip]
        public string FullName => FirstName + " " + LastName;


        [DateOnlyEquality]
        public DateTime? BirthDate { get; set; }
    }
}
