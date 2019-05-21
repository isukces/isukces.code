using isukces.code.interfaces;
using JetBrains.Annotations;

namespace EqualityGeneratorSample
{
    [Auto.EqualityGeneratorAttribute]
    public partial class VerySimpleClassOwner
    {
        public VerySimpleClass    A { get; set; }
        public ClassWithNullables B { get; set; }

        [NotNull]
        public VerySimpleClass ANotNull { get; set; }
        [NotNull]
        public ClassWithNullables BNotNull { get; set; }
    }
}