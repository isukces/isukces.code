
using iSukces.Code.Interfaces;

namespace EqualityGeneratorSample
{
    [Auto.EqualityGeneratorAttribute]
    [Auto.ComparerGeneratorAttribute(nameof(Normal), nameof(NullLikeEmpty))]
    public partial class ClassWithStrings
    {
        public string Normal { get; set; }


        [Auto.NullIsEmptyAttribute]
        public string NullLikeEmpty { get; set; }
    }
}