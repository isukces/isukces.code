using System;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;
using Xunit;

namespace iSukces.Code.Tests;

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
// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
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
// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
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


    [Fact]
    public void T03_Should_create_CopyFromConstructor()
    {
        var g       = new CopyFromGenerator();
        var context = new TestContext();
        g.Generate(typeof(CopyFromConstructor), context);
        var expected = @"
// ReSharper disable All
// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace iSukces.Code.Tests
{
    partial class CopyFromGeneratorTests
    {
        partial class CopyFromConstructor : ICloneable
        {
            /// <summary>
            /// Makes clone of object
            /// </summary>
            public object Clone()
            {
                var a = new CopyFromConstructor(Serial, Name);
                a.CopyFrom(this);
                return a;
            }

            public void CopyFrom(CopyFromConstructor source)
            {
                if (ReferenceEquals(source, null))
                    throw new ArgumentNullException(nameof(source));
            }

        }

    }
}
";
        var actual = context.Code.Trim();

        Assert.Equal(expected.Trim(), actual);
    }

    [Fact]
    public void T04_Should_create_UseSpecialMethod()
    {
        var g       = new CopyFromGenerator();
        var context = new TestContext();
        g.Generate(typeof(UseSpecialMethod), context);
        var expected = @"
// ReSharper disable All
// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace iSukces.Code.Tests
{
    partial class CopyFromGeneratorTests
    {
        partial class UseSpecialMethod : ICloneable
        {
            /// <summary>
            /// Makes clone of object
            /// </summary>
            public object Clone()
            {
                var a = new UseSpecialMethod();
                a.CopyFrom(this);
                return a;
            }

            public void CopyFrom(UseSpecialMethod source)
            {
                if (ReferenceEquals(source, null))
                    throw new ArgumentNullException(nameof(source));
                CopySerialMethod(new iSukces.Code.AutoCode.CopyPropertyValueArgs(source, this, nameof(Serial)));
            }

        }

    }
}
";
        var actual = context.Code.Trim();

        Assert.Equal(expected.Trim(), actual);
    }

    [Fact]
    public void T03_Should_find_one_dimension_array_element_to_copy()
    {
        var tt = CopyFromGenerator.TryGetRankOneArrayElement(typeof(double[]));
        Assert.Equal(typeof(double), tt);
        tt = CopyFromGenerator.TryGetRankOneArrayElement(typeof(int[]));
        Assert.Equal(typeof(int), tt);
        tt = CopyFromGenerator.TryGetRankOneArrayElement(typeof(string[]));
        Assert.Equal(typeof(string), tt);
        tt = CopyFromGenerator.TryGetRankOneArrayElement(typeof(Guid[]));
        Assert.Equal(typeof(Guid), tt);
        
        tt = CopyFromGenerator.TryGetRankOneArrayElement(typeof(object[]));
        Assert.Null(tt);
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
    private class CloneableContainer
    {
        public ExplicitCloneable Explicit { get; set; }
        public ImplicitCloneable Implicit { get; set; }
    }


    [Auto.CopyFromAttribute]
    [Auto.CloneableAttribute]
    private class CopyFromConstructor
    {
        [Auto.CloneableConstructor]
        public CopyFromConstructor(int serial, string name)
        {
            Serial = serial;
            Name   = name;
        }

        public int    Serial { get; }
        public string Name   { get; }
    }

                
    [Auto.CopyFromAttribute]
    [Auto.CloneableAttribute]
    private class UseSpecialMethod
    {
        [Auto.CopyFromByMethod(typeof(UseSpecialMethod), "CopySerialMethod")]
        public CopyFromConstructor Serial { get; set; }
    }
}