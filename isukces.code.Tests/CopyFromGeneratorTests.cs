using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;
using Xunit;

namespace iSukces.Code.Tests
{
    public class CopyFromGeneratorTests
    {
        [Fact]
        public void T01_Should_create_for_nested_class()
        {
            var g                        = new CopyFromGenerator();
            var context = new TestContext();
            g.Generate(typeof(Nested), context);
            var expected = @"// ReSharper disable All
namespace iSukces.Code.Tests
{
    partial class CopyFromGeneratorTests
    {
        partial class Nested : ICloneable
        {
            /// <summary>
            /// Makes clone of object
            /// </summary>
            public object Clone()
            {
                var a = new Nested();
                a.CopyFrom(this);
                return a;
            }

            public void CopyFrom(Nested source)
            {
                if (ReferenceEquals(source, null))
                    throw new ArgumentNullException(nameof(source));
                Number = source.Number; // int
            }

        }

    }
}";
            Assert.Equal(expected.Trim(), context.Code.Trim());
        }

        [Auto.CopyFrom]
        [Auto.CloneableAttribute]
        class Nested
        {
            public int Number { get; set; }
        }
    }
}