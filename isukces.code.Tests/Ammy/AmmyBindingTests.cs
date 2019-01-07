using isukces.code.Ammy;
using isukces.code.interfaces.Ammy;
using Xunit;

namespace isukces.code.Tests.Ammy
{
    public class AmmyBindingTests
    {
        [Fact]
        public void T01_should_create_simple_binding()
        {
            var a = new AmmyBindBuilder("PropName")
                .Build();            
            var writer = new AmmyCodeWriter();
            IConversionCtx ctx= new ConversionCtx(writer);
            a.AppendTo(writer, ctx);
            const string expected = @"bind ""PropName""";
            Assert.Equal(expected, writer.Code);
        }
        [Fact]
        public void T02_should_create_simple_binding_with_mode()
        {
            var a = new AmmyBindBuilder("PropName")
                .WithMode(DataBindingMode.OneTime)
                .Build();            
            var            writer = new AmmyCodeWriter();
            IConversionCtx ctx    = new ConversionCtx(writer);
            a.AppendTo(writer, ctx);
            const string expected = @"bind ""PropName"" set [Mode: OneTime]";
            Assert.Equal(expected, writer.Code);
        }
        [Fact]
        public void T03_should_create_simple_binding_with_mode_and_converter()
        {
            var a = new AmmyBindBuilder("PropName")
                .WithMode(DataBindingMode.OneTime)
                .WithConverter(new AmmyStaticResource("MyConverter"))
                .Build();            
            var            writer = new AmmyCodeWriter();
            IConversionCtx ctx    = new ConversionCtx(writer);
            a.AppendTo(writer, ctx);
            const string expected = @"bind ""PropName"" set [Mode: OneTime, Converter: resource ""MyConverter""]";
            Assert.Equal(expected, writer.Code);
        }
        
        [Fact]
        public void T04_should_create_binding_with_validation_rule()
        {
            var vr = new AmmyObjectBuilder<SampleValidationRule>()
                .WithProperty(aa => aa.MinValue, 12);
            var a = new AmmyBindBuilder("PropName")                
                .WithValidationRule(vr)
                .Build();            
            var            writer = new AmmyCodeWriter();
            IConversionCtx ctx    = new ConversionCtx(writer);
            a.AppendTo(writer, ctx);
            const string expected = @"bind ""PropName"" set [ValidationRules: isukces.code.Tests.Ammy.AmmyBindingTests.SampleValidationRule { MinValue: 12 }]";
            Assert.Equal(expected, writer.Code);
        }
        
        [Fact]
        public void T05_should_create_binding_with_pretty_formatting()
        {
            var vr = new AmmyObjectBuilder<SampleValidationRule>()                
                .WithProperty(aa => aa.MinValue, 12);
            var a = new AmmyBindBuilder("PropName")                
                .WithValidationRule(vr)
                .WithFromAncestor<AmmyBindingTests>()
                .Build();            
            var            writer = new AmmyCodeWriter();
            ConversionCtx ctx    = new ConversionCtx(writer);
            ctx.OnResolveSeparateLines += (source, b) =>
            {
                if (b.Code is IComplexAmmyCodePiece)
                    b.WriteInSeparateLines = true;
            };
            a.AppendTo(writer, ctx);
            const string expected = @"bind ""PropName"" from $ancestor<isukces.code.Tests.Ammy.AmmyBindingTests>
set [
    ValidationRules: isukces.code.Tests.Ammy.AmmyBindingTests.SampleValidationRule { MinValue: 12 }
]";
            Assert.Equal(expected, writer.Code);
        }

        class SampleValidationRule
        {
            public int MinValue { get; set; }
        }
        
    }
}