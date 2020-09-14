using iSukces.Code.Ammy;
using iSukces.Code.Compatibility.System.Windows.Data;
using iSukces.Code.Interfaces.Ammy;
using Xunit;

namespace iSukces.Code.Tests.Ammy
{
    public class AmmyFormatingTests
    {
        private static AmmyObjectBuilder<DoubleValidation> MekeDoubleValidation(string valueName, bool? canBeNull,
            double? minValue = null,
            double? maxValue = null)
        {
            var result = new AmmyObjectBuilder<DoubleValidation>()
                .WithPropertyGenericNotNull(a => a.CanBeNull, canBeNull)
                .WithPropertyGenericNotNull(a => a.MinValue, minValue)
                .WithPropertyGenericNotNull(a => a.MaxValue, maxValue)
                .WithPropertyGenericNotNull(a => a.ValueName, valueName);
            return result;
        }

        [Fact]
        public void T01_should_format_binding_with_mode_and_validation()
        {
            var writer = new AmmyCodeWriter();
            var ctx    = new ConversionCtx(writer);
            ctx.OnResolveSeparateLines += AmmyPretty.VeryPretty;

            var text = new AmmyBindBuilder("CustomHeight")
                .WithMode(XBindingMode.TwoWay)
                .WithValidationRule(MekeDoubleValidation("Custom height", false, 500, 6200))
                .Build();

            var tb = new AmmyObjectBuilder<TextBox>()
                .WithProperty(a => a.Text, text);

            tb.AppendTo(writer, ctx);
            var expected = @"iSukces.Code.Tests.Ammy.TextBox {
    Text: bind ""CustomHeight""
        set [
            Mode: TwoWay
            ValidationRules: iSukces.Code.Tests.Ammy.DoubleValidation { CanBeNull: false, MinValue: 500, MaxValue: 6200, ValueName: ""Custom height"" }
        ]
}";
            Assert.Equal(expected, writer.Code);
        }

        [Fact]
        public void T02_should_format_binding_with_mode_and_validation_and_converter()
        {
            var writer = new AmmyCodeWriter();
            var ctx    = new ConversionCtx(writer);
            ctx.OnResolveSeparateLines += AmmyPretty.VeryPretty;

            var text = new AmmyBindBuilder("CustomHeight")
                .WithMode(XBindingMode.TwoWay)
                .WithConverterFromResource("MyConverter")
                .WithValidationRule(MekeDoubleValidation("Custom height", false, 500, 6200))
                .Build();

            var tb = new AmmyObjectBuilder<TextBox>()
                .WithProperty(a => a.Text, text);

            tb.AppendTo(writer, ctx);
            var expected = @"iSukces.Code.Tests.Ammy.TextBox {
    Text: bind ""CustomHeight""
        set [
            Mode: TwoWay
            Converter: resource ""MyConverter""
            ValidationRules: iSukces.Code.Tests.Ammy.DoubleValidation { CanBeNull: false, MinValue: 500, MaxValue: 6200, ValueName: ""Custom height"" }
        ]
}";
            Assert.Equal(expected, writer.Code);
        }

        [Fact]
        public void T03_should_format_binding_with_mode_and_converter()
        {
            var writer = new AmmyCodeWriter();
            var ctx    = new ConversionCtx(writer);
            ctx.OnResolveSeparateLines += AmmyPretty.VeryPretty;

            var text = new AmmyBindBuilder("CustomHeight")
                .WithMode(XBindingMode.TwoWay)
                .WithConverterFromResource("MyConverter")
                .Build();

            var tb = new AmmyObjectBuilder<TextBox>()
                .WithProperty(a => a.Text, text);

            tb.AppendTo(writer, ctx);
            var expected = @"iSukces.Code.Tests.Ammy.TextBox {
    Text: bind ""CustomHeight"" set [Mode: TwoWay, Converter: resource ""MyConverter""]
}";
            Assert.Equal(expected, writer.Code);
        }
    }
}