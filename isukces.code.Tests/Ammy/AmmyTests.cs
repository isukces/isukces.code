using isukces.code.Ammy;
using isukces.code.interfaces;
using isukces.code.interfaces.Ammy;
using Xunit;

namespace isukces.code.Tests.Ammy
{
    public class AmmyTests
    {
        [Fact]
        public void T01_should_create_simple_code()
        {
            var b = new AmmyObjectBuilder<object>();
            b.WithProperty("Number", 1);                       
            b.WithProperty("Text", "bla");
            var writer = new AmmyCodeWriter();
            var ctx= new ConversionCtx(writer);                        
            b.AppendTo(writer, ctx);
            Assert.Equal("System.Object { Number: 1, Text: \"bla\" }", writer.GetCodeTrim());
            
            // every in separate line
            writer   = new AmmyCodeWriter();
            ctx = new ConversionCtx(writer);
            ctx.OnResolveSeparateLines += (a, bb) => { bb.WriteInSeparateLines = true; };            
            b.AppendTo(writer, ctx);
            var expected = @"System.Object {
    Number: 1
    Text: ""bla""
}";
            Assert.Equal(expected, writer.GetCodeTrim());
        }

        [Fact]
        public void T02_should_create_two_level_object()
        {
            var writer = new AmmyCodeWriter();
            var ctx    = new ConversionCtx(writer);
            ctx.OnResolveSeparateLines += (a, bb) => { bb.WriteInSeparateLines = true; };
            
            
            var b = new AmmyObjectBuilder<object>();
            var nested = new AmmyObjectBuilder<object>();
            b.WithProperty("Number", 1);
            b.WithProperty("Other", 2);
            b.WithProperty("Nested", nested);

            b.AppendTo(writer, ctx);
            
            var expected = @"System.Object {
    Number: 1
    Other: 2
    Nested: System.Object {}
}";
            Assert.Equal(expected, writer.GetCodeTrim());
        }

        [Fact]
        public void T03_should_convert_array()
        {
            var writer = new AmmyCodeWriter();
            var ctx    = new ConversionCtx(writer);
            ctx.OnResolveSeparateLines += (a, bb) => { bb.WriteInSeparateLines = true; };
            
            var b = new AmmyArray();
            b.Items.Add("jeden");
            b.Items.Add("dwa");
            b.Items.Add("trzy");
            b.AppendTo(writer, ctx);
            const string expected = @"[
    ""jeden""
    ""dwa""
    ""trzy""
]";
            Assert.Equal(expected, writer.GetCodeTrim());
        }

        [Fact]
        public void T04_should_convert_arguments()
        {
            var writer = new AmmyCodeWriter();
            var ctx    = new ConversionCtx(writer);

            var result = ctx.ConvertArguments((obj, i) => obj == null, 1, null, 3, null, null);
            Assert.NotNull(result);
            Assert.Equal(3, result.Length);
            foreach (var i in result)
            {
                writer.AppendCodePiece(i);
                writer.Append(",");
            }

            Assert.Equal("1,none,3,", writer.Code);
        }
        
        [Fact]
        public void T05_should_convert_arguments()
        {
            var writer = new AmmyCodeWriter();
            var ctx    = new ConversionCtx(writer);

            var result = ctx.ConvertArguments(null, 1, null, 3, null, null);
            Assert.NotNull(result);
            Assert.Equal(5, result.Length);
            foreach (var i in result)
            {
                writer.AppendCodePiece(i);
                writer.Append(",");
            }

            Assert.Equal("1,none,3,none,none,", writer.Code);
        }

    }
}