using isukces.code.Ammy;
using isukces.code.interfaces.Ammy;
using Xunit;

namespace isukces.code.Tests.Ammy
{
    public class AmmyStaticResourceTests
    {
        [Fact]
        public void T01_Should_create_simple()
        {
            var            a      = new AmmyStaticResource("Bla");
            var            writer = new AmmyCodeWriter();
            IConversionCtx ctx    = new ConversionCtx(writer);
            a.AppendTo(writer, ctx);
            const string expected = @"resource ""Bla""";
            Assert.Equal(expected, writer.Code);
        }

        [Fact]
        public void T02_Should_create_complex()
        {
            var            a      = new AmmyStaticResource("Bla") {EmitAsObject = true};
            var            writer = new AmmyCodeWriter();
            IConversionCtx ctx    = new ConversionCtx(writer);
            a.AppendTo(writer, ctx);
            const string expected = @"StaticResource { ResourceKey: ""Bla"" }";
            Assert.Equal(expected, writer.Code);
        }
    }
}