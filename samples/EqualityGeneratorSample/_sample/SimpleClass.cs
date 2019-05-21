using System;
using isukces.code.interfaces;

namespace EqualityGeneratorSample
{
    [Auto.EqualityGeneratorAttribute]
    public partial class SimpleClass
    {
        public string    FirstName { get; set; }
        public string    LastName  { get; set; }
        public DateTime  BirthDate { get; set; }
        public DateTime? OtherDate { get; set; }
    }
}