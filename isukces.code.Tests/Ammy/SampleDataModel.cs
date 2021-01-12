namespace iSukces.Code.Tests.Ammy
{
    public class SampleDataModel
    {
        public string Name { get; set; }
    }

    public class SampleNestedClass
    {
        public Alpha AlphaValue { get; set; }
        public class Alpha
        {
            public Beta BetaValue { get; set; }
        }

        public class Beta
        {
        
        }
    }
}