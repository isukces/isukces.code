#nullable disable
using System;
using iSukces.Code;
using iSukces.Code.AutoCode;
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
            Assert.Equal(CsNamespaceMemberKind.Class, cl.Kind);
            Assert.True(cl.IsPartial);
        }

        [Fact]
        public void T02_should_create_interface()
        {
            var                       gen = new Gen();
            IAutoCodeGeneratorContext ctx = new TestContext();
            var                       cl  = gen.Make(typeof(SampleInterface), ctx);
            Assert.NotNull(cl);
            Assert.Equal(CsNamespaceMemberKind.Interface, cl.Kind);
            Assert.True(cl.IsPartial);
        }

        [Fact]
        public void T03_should_create_struct()
        {
            var                       gen = new Gen();
            IAutoCodeGeneratorContext ctx = new TestContext();
            var                       cl  = gen.Make(typeof(SampleStruct), ctx);
            Assert.NotNull(cl);
            Assert.Equal(CsNamespaceMemberKind.Struct, cl.Kind);
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
            var    type      = typeof(ShouldSerializeGeneratorTestClass);
            var    pi        = type.GetProperty(propertyName);
            var    generator = new Generators.ShouldSerializeGenerator();
            IAutoCodeGeneratorContext ctx= new TestContext();
            generator.Setup(type, ctx);
            var code      = generator.MakeShouldSerializeCondition(pi);
            Assert.Equal(expectedCode, code);

            pi   = type.GetProperty(nameof(ShouldSerializeGeneratorTestClass.OtherValue));
            code = generator.MakeShouldSerializeCondition(pi);
            Assert.Equal("!OtherValue.Equals(Foo)", code);
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
