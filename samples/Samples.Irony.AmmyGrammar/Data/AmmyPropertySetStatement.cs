namespace Samples.Irony.AmmyGrammar.Data
{
    public partial class AmmyPropertySetStatement
    {
        public override string ToString()
        {
            return $"{PropertyName} = {PropertyValue}";
        }
    }
}