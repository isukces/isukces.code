using Xunit;

namespace Bitbrains.AmmyParser.Tests
{
    public class UnitTest1 : AmmyParserTestsBase
    {
        [Fact]
        public void T01_Should_Parse()
        {
            var sourceCode = @"using Telerik.Windows.Controls
using Telerik.Windows.Controls.Data.PropertyGrid
using Pd.Cad.Wpf
using Pd.Common


mixin CtrlCadBusSegmentSplit_PipeItems() for Pd.Cad.Wpf.PipeItemsControl {
    Jeden: 12
    Dwa: 3
    MinDwa: -3
    Trzy: 3.3
    Cztery: 3.3
    Miasto: ""Okinawa""
        
}
";
            var language = GetLanguageData();
            // var empty    = ParseTree(language, "");
            var o = ParseTree(language, sourceCode);
            Assert.NotNull(o);
        }
    }
}