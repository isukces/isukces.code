using System;
using iSukces.Code.Interfaces;

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