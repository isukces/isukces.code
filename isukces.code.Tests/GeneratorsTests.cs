using System;
using iSukces.Code.Ammy;
using iSukces.Code.AutoCode;
using iSukces.Code.CodeWrite;
using iSukces.Code.Interfaces;
using Xunit;

namespace iSukces.Code.Tests
{
    public class GeneratorsTests
    {
        [Fact]
        public void T01_should_create_class()
        {
            var                       gen = new Gen();
            IAutoCodeGeneratorContext ctx = new TestContext();
            var                       cl  = gen.Make(typeof(GeneratorsTests), ctx);
            Assert.NotNull(cl);
            Assert.Equal(cl.Kind, CsNamespaceMemberKind.Class);
            Assert.True(cl.IsPartial);
        }

        [Fact]
        public void T02_should_create_interface()
        {
            var                       gen = new Gen();
            IAutoCodeGeneratorContext ctx = new TestContext();
            var                       cl  = gen.Make(typeof(SampleInterface), ctx);
            Assert.NotNull(cl);
            Assert.Equal(cl.Kind, CsNamespaceMemberKind.Interface);
            Assert.True(cl.IsPartial);
        }

        [Fact]
        public void T03_should_create_struct()
        {
            var                       gen = new Gen();
            IAutoCodeGeneratorContext ctx = new TestContext();
            var                       cl  = gen.Make(typeof(SampleStruct), ctx);
            Assert.NotNull(cl);
            Assert.Equal(cl.Kind, CsNamespaceMemberKind.Struct);
            Assert.True(cl.IsPartial);
        }

        [Fact]
        public void T04_should_recognize_type()
        {
            Assert.Equal(CsNamespaceMemberKind.Struct, typeof(SampleStruct).GetNamespaceMemberKind());
            Assert.Equal(CsNamespaceMemberKind.Interface, typeof(SampleInterface).GetNamespaceMemberKind());
            Assert.Equal(CsNamespaceMemberKind.Class, typeof(GeneratorsTests).GetNamespaceMemberKind());
        }

        [Theory]
        [InlineData("Number1", "Number1 != 0")]
        [InlineData("Number2", "Number2.HasValue && Number2.Value != 0")]
        [InlineData("Name", "!string.IsNullOrEmpty(Name)")]
        [InlineData("OtherValue", "!OtherValue.Equals(Foo)")]
        public void T05_ShouldSerializeGenerator_tests(string propertyName, string expectedCode)
        {
            var pi   = typeof(ShouldSerializeGeneratorTestClass).GetProperty(propertyName);
            var code = new Generators.ShouldSerializeGenerator().MakeShouldSerializeCondition(pi);
            Assert.Equal(expectedCode, code);

            pi = typeof(ShouldSerializeGeneratorTestClass).GetProperty(
                nameof(ShouldSerializeGeneratorTestClass.OtherValue));
            code = new Generators.ShouldSerializeGenerator().MakeShouldSerializeCondition(pi);
            Assert.Equal("!OtherValue.Equals(Foo)", code);
        }

        [Fact]
        public void T06_Should_convert_generic_type_name()
        {
            var t  = typeof(AmmyObjectBuilder<>);
            var tn = new CsFile().GetTypeName(t);
            Assert.Equal("iSukces.Code.Ammy.AmmyObjectBuilder<TPropertyBrowser>", tn);
        }

        [Auto.ShouldSerializeInfoAttribute("!{0}.Equals(Foo)")]
        public struct SampleStruct
        {
            public int Number { get; set; }
        }

        private interface SampleInterface
        {
        }

        public class ShouldSerializeGeneratorTestClass
        {
            public int          Number1    { get; set; }
            public int?         Number2    { get; set; }
            public string       Name       { get; set; }
            public SampleStruct OtherValue { get; set; }
        }

        private class Gen : Generators.SingleClassGenerator
        {
            public CsClass Make(Type type, IAutoCodeGeneratorContext context)
            {
                Setup(type, context);
                return Class;
            }

            protected override void GenerateInternal()
            {
                throw new NotImplementedException();
            }
        }
    }
}