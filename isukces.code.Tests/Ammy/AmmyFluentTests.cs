#if AMMY
using iSukces.Code.Ammy;
using iSukces.Code.Compatibility.System.Windows.Data;
using iSukces.Code.Compatibility.Telerik;
using iSukces.Code.Interfaces.Ammy;
using Xunit;

namespace iSukces.Code.Tests.Ammy
{
    public class AmmyFluentTests
    {
        private static string EmitCode(IAmmyCodePieceConvertible tb)
        {
            var nsProvider = new AmmyNamespaceProvider();
            nsProvider.Namespaces.Add("System.Windows");
            nsProvider.Namespaces.Add("iSukces.Code.Tests.Ammy");
            var ctx = new ConversionCtx(nsProvider);
            ctx.OnResolveSeparateLines += (sender, args) =>
            {
                args.WriteInSeparateLines = true;
                args.Handled              = true;
            };
            var writer = new AmmyCodeWriter();
            tb.AppendTo(writer, ctx);
            return writer.FullCode;
        }

#if AMMY
        [Fact]
        public void T01_Should_create_code_with_fluent_from_static_resource()
        {
            var tb = new AmmyObjectBuilder<SomeWpfControl>();
            tb.WithPropertyStaticResource(a => a.Style, "CoolDialogRadMaskedNumericInputStyle")
                .WithProperty(a => a.Height, 33);

            var code = EmitCode(tb);
            const string expected = @"AmmyFluentTests.SomeWpfControl {
    Style: resource ""CoolDialogRadMaskedNumericInputStyle""
    Height: 33
}";
            Assert.Equal(expected, code);
        }

        [Fact]
        public void T02_Should_create_code_with_fluent()
        {
            var builder = new AmmyObjectBuilder<SomeWpfControl>();
            var wrapper = builder
                .ExtendAny<AmmyObjectBuilder<SomeWpfControl>, SomeWpfControl>();
            wrapper
                .WithPropertyStaticResource(a => a.Style, "CoolDialogRadMaskedNumericInputStyle")
                .WithProperty(a => a.Height, 33);

            var code = EmitCode(wrapper);
            const string expected = @"AmmyFluentTests.SomeWpfControl {
    Style: resource ""CoolDialogRadMaskedNumericInputStyle""
    Height: 33
}";
            Assert.Equal(expected, code);
        }

        [Fact]
        public void T03_Should_create_code_with_direct_data_context()
        {
            var builder = new AmmyObjectBuilder<SomeWpfControl>();
            var wrapper = builder
                .Extend()
                .WithDataContext<SomeModel>(new AmmyBind("Bla"))
                .Bind(a => a.Content, a => a.Name)
                .CloseDataContext()
                .Builder;

            var code = EmitCode(wrapper);
            const string expected = @"AmmyFluentTests.SomeWpfControl {
    DataContext: bind ""Bla""
    Content: bind ""Name""
}";
            Assert.Equal(expected, code);
        }

        [Fact]
        public void T04_Should_create_code_with_data_context_from_ancestor_by_expression()
        {
            var builder = new AmmyObjectBuilder<SomeWpfControl>();
            var wrapper = builder
                .Extend()
                .WithDataContextFromAncestor<RadGridView>()
                .Bind(a => a.Content, a => a.VerticalAlignment);

            var code = EmitCode(wrapper);
            const string expected = @"AmmyFluentTests.SomeWpfControl {
    DataContext: bind from $ancestor<Telerik.Windows.Controls.RadGridView>
        set [
            Mode: OneWay
        ]
    Content: bind ""VerticalAlignment""
}";
            Assert.Equal(expected, code);
        }

        [Fact]
        public void T05_Should_create_code_with_data_context_from_ancestor_by_name()
        {
            var builder = new AmmyObjectBuilder<SomeWpfControl>();
            var wrapper = builder
                .Extend()
                .WithDataContextFromAncestor<RadGridView>()
                .Bind(a => a.Content, "Kuku");

            var code = EmitCode(wrapper);
            const string expected = @"AmmyFluentTests.SomeWpfControl {
    DataContext: bind from $ancestor<Telerik.Windows.Controls.RadGridView>
        set [
            Mode: OneWay
        ]
    Content: bind ""Kuku""
}";
            Assert.Equal(expected, code);
        }

        [Fact]
        public void T06_Should_create_code_with_data_context_from_ancestor_by_names()
        {
            var builder = new AmmyObjectBuilder<SomeWpfControl>();
            var wrapper = builder
                .Extend()
                .WithDataContextFromAncestor<RadGridView>()
                .Bind("Content", "Kuku");

            var code = EmitCode(wrapper);
            const string expected = @"AmmyFluentTests.SomeWpfControl {
    DataContext: bind from $ancestor<Telerik.Windows.Controls.RadGridView>
        set [
            Mode: OneWay
        ]
    Content: bind ""Kuku""
}";
            Assert.Equal(expected, code);
        }

        [Fact]
        public void T07_Should_create_code_with_data_context_from_Element_name()
        {
            var builder = new AmmyObjectBuilder<SomeWpfControl>();
            var wrapper = builder
                .Extend()
                .WithDataContextFromFromElementName<SomeOtherWpfControl, SomeWpfControl>("OtherElementName",
                    a => a.Content)
                .Bind(a => a.Content, a => a.Height);

            var code = EmitCode(wrapper);
            const string expected = @"AmmyFluentTests.SomeWpfControl {
    DataContext: bind ""Content"" from ""OtherElementName""
        set [
            Mode: OneWay
        ]
    Content: bind ""Height""
}";
            Assert.Equal(expected, code);
        }

        [Fact]
        public void T08_Should_create_code_with_data_context_silent()
        {
            var builder = new AmmyObjectBuilder<SomeWpfControl>();
            var wrapper = builder
                .Extend()
                .WithDataContextSilent<SomeModel>()
                .Bind(a => a.Content, a => a.Income);

            var code = EmitCode(wrapper);
            const string expected = @"AmmyFluentTests.SomeWpfControl {
    Content: bind ""Income""
}";
            Assert.Equal(expected, code);
        }


        [Fact]
        public void T09_Should_create_code_with_data_context_browsed_from_top_level_object()
        {
            var builder = new AmmyObjectBuilder<SomeWpfControl>();
            var wrapper = builder
                .Extend()
                .Browse<SomeOtherWpfControl>()
                .ChooseElement(a => a.Content)
                .WithDataContext(a => a)
                .Bind(a => a.Content, a => a.Style);

            var code = EmitCode(wrapper);
            const string expected = @"AmmyFluentTests.SomeWpfControl {
    DataContext: bind from ""Content""
        set [
            Mode: OneWay
        ]
    Content: bind ""Style""
}";
            Assert.Equal(expected, code);
        }

        [Fact]
        public void T10_Should_create_code_with_data_context_browsed_from_top_level_object()
        {
            var builder = new AmmyObjectBuilder<FakeTextBlock>();
            var wrapper = builder
                .Extend()
                .Browse<SomeOtherWpfControl>()
                .ChooseElement(a => a.Content)
                .BindFromElement(a => a.FontFamily, a => a.X2, XBindingMode.OneTime);

            var code = EmitCode(wrapper);
            const string expected = @"FakeTextBlock {
    FontFamily: bind ""X2"" from ""Content""
        set [
            Mode: OneTime
        ]
}";
            Assert.Equal(expected, code);
        }
#endif


        private class SomeOtherWpfControl
        {
            public SomeWpfControl Content { get; set; }
            public string         X1      { get; set; }
        }

        private class SomeWpfControl
        {
            public object Content { get; set; }
            public object Style   { get; set; }
            public double Height  { get; set; }
            public string X2      { get; set; }
        }

        private class SomeModel
        {
            public string Name   { get; set; }
            public double Income { get; set; }
            public string X3     { get; set; }
        }
    }
}
#endif