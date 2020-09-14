using iSukces.Code.Ammy;
using iSukces.Code.Interfaces;
using iSukces.Code.Interfaces.Ammy;
using Xunit;

namespace iSukces.Code.Tests.Ammy
{
    public class AmmySystemColorDynamicResourceTests
    {
        [Theory]
        [InlineData(false, "resource dyn SystemColors.WindowBrushKey")]
        [InlineData(true, "resource dyn System.Windows.SystemColors.WindowBrushKey")]
        public void T01_Should_create_dynamic_resource(bool fullNamespaces, string expected)
        {
            var resource = new AmmySystemColorDynamicResource(SystemColorsKeys.WindowBrush);
            var nsProvider = new AmmyNamespaceProvider();
            nsProvider.Namespaces.Add("System.Windows");
            IConversionCtx ctx  = new ConversionCtx(nsProvider, fullNamespaces);
            var            code = resource.ToAmmyCode(ctx) as SimpleAmmyCodePiece;
            Assert.Equal(expected, code.Code);
        }

        [Fact]
        public void T02_Should_create_mixin()
        {
            var mixinBuilder = new MixinBuilder<object>("mixinName");
            
            var resource = new AmmySystemColorDynamicResource(SystemColorsKeys.WindowBrush);
            mixinBuilder.WithProperty("Brush1", resource);
            mixinBuilder.WithProperty("Brush2", SystemColorsKeys.WindowBrush);
            
            var nsProvider = new AmmyNamespaceProvider();
            nsProvider.Namespaces.Add("System.Windows");
            var ctx = new ConversionCtx(nsProvider);
            ctx.OnResolveSeparateLines += AmmyPretty.VeryPretty;

            var writer= new AmmyCodeWriter();
            mixinBuilder.WrappedMixin.AppendTo(writer, ctx);
            var code = writer.FullCode;
            string expected = @"mixin mixinName() for object {
    Brush1: resource dyn SystemColors.WindowBrushKey
    Brush2: resource dyn SystemColors.WindowBrushKey
}";
            var expectedcode = code.CsVerbatimEncode();
            Assert.Equal(expected,code);
        }
    }
}