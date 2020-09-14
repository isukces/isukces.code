using iSukces.Code.Ammy;
using iSukces.Code.Interfaces;
using iSukces.Code.Interfaces.Ammy;
using Xunit;

namespace iSukces.Code.Tests.Ammy
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
            var writer = new AmmyCodeWriter();
            IConversionCtx ctx = new ConversionCtx(writer);

            var a1 = new AmmyStaticResource("Bla");
            var a = a1.GetObjectSyntaxCode(ctx);
            writer.AppendComplex(a);
            const string expected = @"StaticResource { ResourceKey: ""Bla"" }";
            Assert.Equal(expected, writer.Code);
        }
    }
}