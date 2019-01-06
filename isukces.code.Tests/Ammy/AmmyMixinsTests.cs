using isukces.code.Ammy;
using isukces.code.interfaces;
using isukces.code.interfaces.Ammy;
using Xunit;

namespace isukces.code.Tests.Ammy
{
    public class AmmyMixinsTests
    {
        [Fact]
        public void T01_should_create_mixin_from_builder()
        {
            var m = new MixinBuilder<SampleWindowsControls>("test")
                {                    
                }
                .WithPropertyGeneric(a => a.Width, 10)
                .WithProperty(a => a.Heigth, new AmmyVariable("standardHeight"))
                .WithPropertyAncestorBind<SampleUserControl>(a => a.Content, a => a.Model.FirstName);

            var writer = new AmmyCodeWriter();
            var ctx    = new ConversionCtx(writer);
            ctx.OnResolveSeparateLines += (a, bb) => { bb.WriteInSeparateLines = true; };
            m.AppendTo(writer, ctx);

            const string expected = @"mixin test() for isukces.code.Tests.Ammy.AmmyMixinsTests.SampleWindowsControls {
    Width: 10
    Heigth: $standardHeight
    Content: bind ""Model.FirstName"" from $ancestor<isukces.code.Tests.Ammy.AmmyMixinsTests.SampleUserControl>
}";
            Assert.Equal(expected, writer.GetCodeTrim());
        }
        
        [Fact]
        public void T02_should_create_mixin_from_builder_with_short_type_name()
        {
            var m = new MixinBuilder<SampleWindowsControls>("test")
                .WithPropertyGeneric(a => a.Width, 10)
                .WithProperty(a => a.Heigth, new AmmyVariable("standardHeight"))
                .WithPropertyAncestorBind<SampleUserControl>(a => a.Content, a => a.Model.FirstName);

            var writer = new AmmyCodeWriter();
            writer.AddNamespace<SampleUserControl>();
            var ctx    = new ConversionCtx(writer);
            ctx.OnResolveSeparateLines += (a, bb) => { bb.WriteInSeparateLines = true; };
            m.AppendTo(writer, ctx);

            const string expected = @"mixin test() for AmmyMixinsTests.SampleWindowsControls {
    Width: 10
    Heigth: $standardHeight
    Content: bind ""Model.FirstName"" from $ancestor<AmmyMixinsTests.SampleUserControl>
}";
            Assert.Equal(expected, writer.GetCodeTrim());
        }

        public class SampleWindowsControls
        {
            public int    Width   { get; set; }
            public int    Heigth  { get; set; }
            public object Content { get; set; }
        }

        public class SampleUserControl
        {
            public SampleModel Model { get; set; }
        }

        public class SampleModel
        {
            public string FirstName { get; set; }
            public string LastName  { get; set; }
        }
    }
}