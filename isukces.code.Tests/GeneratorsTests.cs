using System;
using System.Collections.Generic;
using isukces.code.AutoCode;
using isukces.code.CodeWrite;
using isukces.code.interfaces;
using Xunit;

namespace isukces.code.Tests
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
            Assert.Equal(cl.Kind, NamespaceMemberKind.Class);
            Assert.True(cl.IsPartial);
        }

        [Fact]
        public void T02_should_create_interface()
        {
            var                       gen = new Gen();
            IAutoCodeGeneratorContext ctx = new TestContext();
            var                       cl  = gen.Make(typeof(SampleInterface), ctx);
            Assert.NotNull(cl);
            Assert.Equal(cl.Kind, NamespaceMemberKind.Interface);
            Assert.True(cl.IsPartial);
        }

        [Fact]
        public void T03_should_create_struct()
        {
            var                       gen = new Gen();
            IAutoCodeGeneratorContext ctx = new TestContext();
            var                       cl  = gen.Make(typeof(SampleStruct), ctx);
            Assert.NotNull(cl);
            Assert.Equal(cl.Kind, NamespaceMemberKind.Struct);
            Assert.True(cl.IsPartial);
        }

        [Fact]
        public void T04_should_recognize_type()
        {
            Assert.Equal(NamespaceMemberKind.Struct, typeof(SampleStruct).GetNamespaceMemberKind());
            Assert.Equal(NamespaceMemberKind.Interface, typeof(SampleInterface).GetNamespaceMemberKind());
            Assert.Equal(NamespaceMemberKind.Class, typeof(GeneratorsTests).GetNamespaceMemberKind());
        }

        [Fact]
        public void T05_ShouldSerializeGenerator_tests()
        {
            var pi = typeof(ShouldSerializeGeneratorTestClass).GetProperty(
                nameof(ShouldSerializeGeneratorTestClass.Name));
            var code = Generators.ShouldSerializeGenerator.MakeShouldSerializeCondition(pi);
            Assert.Equal("!string.IsNullOrEmpty(Name)", code);
            
            pi = typeof(ShouldSerializeGeneratorTestClass).GetProperty(
                nameof(ShouldSerializeGeneratorTestClass.OtherValue));
            code = Generators.ShouldSerializeGenerator.MakeShouldSerializeCondition(pi);
            Assert.Equal("!OtherValue.Equals(Foo)", code);
        }

        [Auto.ShouldSerializeInfo("!{0}.Equals(Foo)")]
        public struct SampleStruct
        {
            public int Number { get; set; }
        }

        private interface SampleInterface
        {
        }

        public class ShouldSerializeGeneratorTestClass
        {
            public string Name { get; set; }
            public SampleStruct OtherValue { get; set; }
        }

        private class Gen : Generators.SingleClassGenerator
        {
            public CsClass Make(Type type, IAutoCodeGeneratorContext context)
            {
                Setup(type, context);
                return Class;
            }
        }

        public class TestContext : IAutoCodeGeneratorContext
        {
            public void AddNamespace(string namepace)
            {
                throw new NotImplementedException();
            }

            public CsClass GetOrCreateClass(Type type)
            {
                if (_file == null)
                    _file = new CsFile();
                return _file.GetOrCreateClass(type, new Dictionary<Type, CsClass>());
            }

            public T ResolveConfig<T>() where T : class, IAutoCodeConfiguration, new()
            {
                throw new NotImplementedException();
            }

            private CsFile _file;
        }
    }
}