using System;
using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Ammy;
using iSukces.Code.Compatibility.System.Windows.Data;
using iSukces.Code.Interfaces;
using iSukces.Code.Interfaces.Ammy;
using Xunit;
using Xunit.Abstractions;

namespace iSukces.Code.Tests.Ammy
{
    public class AmmyBindingTests
    {
        public AmmyBindingTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void T01_should_create_simple_binding()
        {
            var a = new AmmyBindBuilder("PropName")
                .Build();
            var            writer = new AmmyCodeWriter();
            IConversionCtx ctx    = new ConversionCtx(writer);
            a.AppendTo(writer, ctx);
            const string expected = @"bind ""PropName""";
            Assert.Equal(expected, writer.Code);
        }

        [Fact]
        public void T02_should_create_simple_binding_with_mode()
        {
            var a = new AmmyBindBuilder("PropName")
                .WithMode(XBindingMode.OneTime)
                .Build();
            var            writer = new AmmyCodeWriter();
            IConversionCtx ctx    = new ConversionCtx(writer);
            a.AppendTo(writer, ctx);
            const string expected = @"bind ""PropName"" set [Mode: OneTime]";
            Assert.Equal(expected, writer.Code);
        }

        [Fact]
        public void T03_should_create_simple_binding_with_mode_and_converter()
        {
            var a = new AmmyBindBuilder("PropName")
                .WithMode(XBindingMode.OneTime)
                .WithConverterFromResource("MyConverter")
                .Build();
            var            writer = new AmmyCodeWriter();
            IConversionCtx ctx    = new ConversionCtx(writer);
            a.AppendTo(writer, ctx);
            const string expected = @"bind ""PropName"" set [Mode: OneTime, Converter: resource ""MyConverter""]";
            Assert.Equal(expected, writer.Code);
        }

        [Fact]
        public void T04_should_create_binding_with_validation_rule()
        {
            var vr = new AmmyObjectBuilder<SampleValidationRule>()
                .WithProperty(aa => aa.MinValue, 12);
            var a = new AmmyBindBuilder("PropName")
                .WithValidationRule(vr)
                .Build();
            var            writer = new AmmyCodeWriter();
            IConversionCtx ctx    = new ConversionCtx(writer);
            a.AppendTo(writer, ctx);
            const string expected =
                @"bind ""PropName"" set [ValidationRules: iSukces.Code.Tests.Ammy.AmmyBindingTests.SampleValidationRule { MinValue: 12 }]";
            Assert.Equal(expected, writer.Code);
        }

        [Fact]
        public void T05_should_create_binding_with_pretty_formatting()
        {
            var vr = new AmmyObjectBuilder<SampleValidationRule>()
                .WithProperty(aa => aa.MinValue, 12);
            var a = new AmmyBindBuilder("PropName")
                .WithValidationRule(vr)
                .WithBindFromAncestor<AmmyBindingTests>()
                .Build();
            var writer = new AmmyCodeWriter();
            var ctx    = new ConversionCtx(writer);
            ctx.OnResolveSeparateLines += (source, b) =>
            {
                if (b.Code is IComplexAmmyCodePiece)
                    b.WriteInSeparateLines = true;
            };
            a.AppendTo(writer, ctx);
            const string expected = @"bind ""PropName"" from $ancestor<iSukces.Code.Tests.Ammy.AmmyBindingTests>
    set [
        ValidationRules: iSukces.Code.Tests.Ammy.AmmyBindingTests.SampleValidationRule { MinValue: 12 }
    ]";
            Assert.Equal(expected, writer.Code);
        }


        [Theory]
        [MemberData(nameof(Data), 2)]
        public void T11_should_create_binding_as_object(AmmyBind bind, string expected)
        {
            var writer = new AmmyCodeWriter();
            var ctx    = new ConversionCtx(writer);
            ctx.OnResolveSeparateLines += (source, b) =>
            {
                if (b.Code is IComplexAmmyCodePiece)
                    b.WriteInSeparateLines = true;
            };
            var bindo = bind.GetObjectSyntaxCode(ctx);
            writer.AppendComplex(bindo);
            _testOutputHelper.WriteLine(expected);
            _testOutputHelper.WriteLine(writer.Code);
            Assert.Equal(expected, writer.Code);
        }

        public static IList<object[]> Data
        {
            get { return Data2.Select(a => new object[] {a.Item1, a.Item2}).ToArray(); }
        }

        public static IEnumerable<Tuple<AmmyBind, string>> Data2
        {
            get
            {
                var bi = new AmmyBindBuilder("PropName")
                    .WithConverter(new AmmyStaticResource("ConverterResource"));
                yield return Tuple.Create(bi.Build(),
                    @"Bind { Path: ""PropName"", Converter: resource ""ConverterResource"" }");

                bi.WithBindFromAncestor<SampleDataModel>();
                yield return Tuple.Create(bi.Build(),
                    @"Bind { Path: ""PropName""
    RelativeSource: RelativeSource { Mode: ""FindAncestor"", AncestorType: ""iSukces.Code.Tests.Ammy.SampleDataModel"" }
 Converter: resource ""ConverterResource""
}");
            }
        }

        private readonly ITestOutputHelper _testOutputHelper;

        private class SampleValidationRule
        {
            public int MinValue { get; set; }
        }
    }
}