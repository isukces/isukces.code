using System;
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
            var g       = new CopyFromGenerator();
            var context = new TestContext();
            g.Generate(typeof(Nested), context);
            var expected = @"


// ReSharper disable All
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
                if (source.Doubles == null)
                    Doubles = null;
                else {
                    var sourceDoubles = source.Doubles;
                    var targetDoubles = new double[sourceDoubles.Length];
                    System.Array.Copy(sourceDoubles, 0, targetDoubles, 0, sourceDoubles.Length);
                    Doubles = targetDoubles;
                }
            }

        }

    }
}

";
            var actual = context.Code.Trim();

            Assert.Equal(expected.Trim(), actual);
        }

        
        
        [Fact]
        public void T02_Should_create_cloneable()
        {
            var g       = new CopyFromGenerator();
            var context = new TestContext();
            g.Generate(typeof(CloneableContainer), context);
            var expected = @"


// ReSharper disable All
namespace iSukces.Code.Tests
{
    partial class CopyFromGeneratorTests
    {
        partial class CloneableContainer : ICloneable
        {
            /// <summary>
            /// Makes clone of object
            /// </summary>
            public object Clone()
            {
                var a = new CloneableContainer();
                a.CopyFrom(this);
                return a;
            }

            public void CopyFrom(CloneableContainer source)
            {
                if (ReferenceEquals(source, null))
                    throw new ArgumentNullException(nameof(source));
                Explicit = (ExplicitCloneable)((ICloneable)source.Explicit)?.Clone();
                Implicit = (ImplicitCloneable)source.Implicit?.Clone();
            }

        }

    }
}

";
            var actual = context.Code.Trim();

            Assert.Equal(expected.Trim(), actual);
        }

        [Auto.CopyFromAttribute]
        [Auto.CloneableAttribute]
        private class Nested
        {
            public int      Number  { get; set; }
            public double[] Doubles { get; set; }
        }

        private class ExplicitCloneable : ICloneable
        {
            object ICloneable.Clone() => MemberwiseClone();

            public int NumberExplicitCloneable { get; set; }
        }

        private class ImplicitCloneable : ICloneable
        {
            public object Clone() => MemberwiseClone();

            public int NumberImplicitCloneable { get; set; }
        }

        
        [Auto.CopyFromAttribute]
        [Auto.CloneableAttribute]
        class CloneableContainer
        {
            public ExplicitCloneable Explicit          { get; set; }
            public ImplicitCloneable Implicit { get; set; }

        }
    }
}