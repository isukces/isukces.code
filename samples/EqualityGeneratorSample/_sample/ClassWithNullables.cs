

using iSukces.Code.Interfaces;

namespace EqualityGeneratorSample
{
    [Auto.EqualityGeneratorAttribute]
    [Auto.ComparerGeneratorAttribute(nameof(Normal), nameof(NullLikeEmpty))]
    public partial class ClassWithNullables
    {
        public int? Normal { get; set; }


        [Auto.NullIsEmptyAttribute]
        public int? NullLikeEmpty { get; set; }

    }
}