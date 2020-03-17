using Xunit;

namespace Bitbrains.AmmyParser.Tests
{
    public class UnitTest1 : AmmyParserTestsBase
    {
        [Fact]
        public void T01_Should_parse_basic_mixin()
        {
            var sourceCode = @"
using Telerik.Windows.Controls

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
        
        [Fact]
        public void T02_Should_parse_basic_mixin_with_comma()
        {
            var sourceCode = @"using Telerik.Windows.Controls
using Pd.Common


mixin CtrlCadBusSegmentSplit_PipeItems() for Pd.Cad.Wpf.PipeItemsControl {
    Jeden: 12, Dwa: 3
    MinDwa: -3
    Trzy: 3.3, Cztery: 3.3
    Miasto: ""Okinawa""        
}
";
            var language = GetLanguageData();
            // var empty    = ParseTree(language, "");
            var o = ParseTree(language, sourceCode);
            Assert.NotNull(o);
        }
        
        [Fact]
        public void T03_Should_parse_basic_mixin_without_usings()
        {
            var sourceCode = @"
mixin CtrlCadBusSegmentSplit_PipeItems() for Pd.Cad.Wpf.PipeItemsControl {
    Jeden: 12, Dwa: 3
    MinDwa: -3
    Trzy: 3.3, Cztery: 3.3
    Miasto: ""Okinawa""        
}
";
            var language = GetLanguageData();
            // var empty    = ParseTree(language, "");
            var o = ParseTree(language, sourceCode);
            Assert.NotNull(o);
        }
        
        [Fact]
        public void T04_Should_parse_mixin_with_context_binding()
        {
            var sourceCode = @"
mixin CtrlCadBusSegmentSplit_PipeItems() for Pd.Cad.Wpf.PipeItemsControl {
    Jeden: 12, Dwa: 3
    MinDwa: -3
    Trzy: 3.3, Cztery: 3.3
    Miasto: ""Okinawa""
    Bind1: bind ""SomeContextProperty""        
}
";
            var language = GetLanguageData();
            // var empty    = ParseTree(language, "");
            var o = ParseTree(language, sourceCode);
            Assert.NotNull(o);
        }
        
        [Fact]
        public void T05_Should_parse_mixin_with_ancestor_binding()
        {
            var sourceCode = @"
mixin CtrlCadBusSegmentSplit_PipeItems() for Pd.Cad.Wpf.PipeItemsControl {
    Jeden: 12, Dwa: 3
    MinDwa: -3
    Trzy: 3.3, Cztery: 3.3
    Miasto: ""Okinawa""
    Bind1: bind ""SomeContextProperty""  from $ancestor<Some.QualifiedName>()      
    Bind3: bind ""SomeContextProperty""  from $ancestor<Some.QualifiedName>(3)
}
";
            var language = GetLanguageData();
            // var empty    = ParseTree(language, "");
            var o = ParseTree(language, sourceCode);
            Assert.NotNull(o);
        }
    }
}