#nullable disable
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;
using Xunit;

namespace iSukces.Code.Tests
{
    public class ShouldSerializeGeneratorTests
    {
        [Fact]
        public void T01_Shoul_generate_should_serialize_with_enum()
        {
            //IMemberNullValueChecker c   = new MyValueChecker();
            var q   = new Generators.ShouldSerializeGenerator();
            var ctx = new TestContext();
            q.Generate(typeof(T1), ctx);
            var expected = @"// ReSharper disable All
// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace iSukces.Code.Tests
{
    partial class ShouldSerializeGeneratorTests
    {
        partial class T1
        {
            public bool ShouldSerializeE() => E != SomeEnum.None;

        }

    }
}
";
            Assert.Equal(expected.Trim(), ctx.Code.Trim());
            //CompareCode(ctx.Code, method, file);
        }

        [Auto.ShouldSerializeInfoAttribute("{0} != {1}.None")]
        private enum SomeEnum
        {
            None,
            One
        }

        private class T1
        {
            [Auto. ShouldSerialize]
            public SomeEnum E { get; set; }
        }
    }
}
