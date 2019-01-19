using isukces.code.Ammy;
using isukces.code.interfaces.Ammy;
using Xunit;

namespace isukces.code.Tests.Ammy
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
                @"mixin MyMixin() for isukces.code.Tests.Ammy.TextBox { Text: bind ""Name"" from $ancestor<isukces.code.Tests.Ammy.SampleDataModel> }";
            Assert.Equal(expected, code);
        }

        [Fact]
        public void T02_should_create_mixin_with_ancestor_twoway_binding()
        {
            var m = new MixinBuilder<TextBox>("MyMixin")
                .WithPropertyAncestorBind<SampleDataModel>(a => a.Text, a => a.Name,
                    bind => { bind.WithMode(DataBindingMode.TwoWay); });
            var code = GenCode(m.WrappedMixin);

            var expected =
                @"mixin MyMixin() for isukces.code.Tests.Ammy.TextBox { Text: bind ""Name"" from $ancestor<isukces.code.Tests.Ammy.SampleDataModel> set [Mode: TwoWay] }";
            Assert.Equal(expected, code);
        }
    }
}