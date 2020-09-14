using iSukces.Code.Ammy;
using iSukces.Code.Compatibility.System.Windows.Data;
using iSukces.Code.Interfaces.Ammy;
using Xunit;

namespace iSukces.Code.Tests.Ammy
{
    public class MixinBuilderTests
    {
        private static string GenCode(IAmmyCodePieceConvertible x)
        {
            var writer = new AmmyCodeWriter();
            var ctx    = new ConversionCtx(writer);

            x.AppendTo(writer, ctx);
            return writer.Code;
        }

        [Fact]
        public void T01_should_create_mixin_with_ancestor_binding()
        {
            var m = new MixinBuilder<TextBox>("MyMixin")
                .WithPropertyAncestorBind<SampleDataModel>(a => a.Text, a => a.Name);
            var code = GenCode(m.WrappedMixin);

            const string expected =
                @"mixin MyMixin() for iSukces.Code.Tests.Ammy.TextBox { Text: bind ""Name"" from $ancestor<iSukces.Code.Tests.Ammy.SampleDataModel> }";
            Assert.Equal(expected, code);
        }

        [Fact]
        public void T02_should_create_mixin_with_ancestor_twoway_binding()
        {
            var m = new MixinBuilder<TextBox>("MyMixin")
                .WithPropertyAncestorBind<SampleDataModel>(a => a.Text, a => a.Name,
                    bind => { bind.WithMode(XBindingMode.TwoWay); });
            var code = GenCode(m.WrappedMixin);

            var expected =
                @"mixin MyMixin() for iSukces.Code.Tests.Ammy.TextBox { Text: bind ""Name"" from $ancestor<iSukces.Code.Tests.Ammy.SampleDataModel> set [Mode: TwoWay] }";
            Assert.Equal(expected, code);
        }
    }
}