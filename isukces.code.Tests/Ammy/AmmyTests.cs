using System.Collections.Generic;
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
            b.WithAny("Number", 1);                       
            b.WithAny("Text", "bla");
            var writer = new AmmyCodeWriter();
            var ctx= new ConversionCtx(writer);                        
            b.WriteTo(writer, ctx);
            Assert.Equal("System.Object { Number: 1, Text: \"bla\" }", writer.GetCodeTrim());
            
            // every in separate line
            writer   = new AmmyCodeWriter();
            ctx = new ConversionCtx(writer);
            ctx.OnResolveSeparateLines += (a, bb) => { bb.WriteInSeparateLines = true; };            
            b.WriteTo(writer, ctx);
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
            b.WithAny("Number", 1);
            b.WithAny("Other", 2);
            b.WithAny("Nested", nested);

            b.WriteTo(writer, ctx);
            
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
            b.WriteTo(writer, ctx);
            const string expected = @"[
    ""jeden""
    ""dwa""
    ""trzy""
]";
            Assert.Equal(expected, writer.GetCodeTrim());
        }
        
    }
}