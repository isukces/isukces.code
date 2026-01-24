#nullable disable
using System.Collections.Generic;
using iSukces.Code;
using iSukces.Code.AutoCode;
using Xunit;

namespace iSukces.Code.Tests;

public class ClassCreationTests
{
    [Fact]
    public void T01_Should_create_class()
    {
        var c     = new CsFile();
        var type  = TypeProvider.FromType(typeof(ParentGeneric<>.Nested));
        c.GetOrCreateClass(type);
        var code = c.GetCode();
        Assert.Equal(@"// ReSharper disable All
// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace iSukces.Code.Tests
{
    partial class ParentGeneric<T>
    {
        partial class Nested
        {
        }

    }
}
", code, ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
    }

    [Fact]
    public void T02_Should_add_comment()
    {
        var c       = new CsFile();
        var type    = TypeProvider.FromType(typeof(ParentGeneric<>.Nested));
        var cache   = new Dictionary<TypeProvider, CsClass>();
        var myClass = c.GetOrCreateClass(type);
        myClass.AddComment("Line");
        {
            var p = myClass.AddProperty("Count", CsType.Int32);
            p.AddComment("line1");
            p.AddComment("line2");
        }
        var code = c.GetCode();
        Assert.Equal(@"// ReSharper disable All
// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace iSukces.Code.Tests
{
    partial class ParentGeneric<T>
    {
        // Line
        partial class Nested
        {
            /*
            line1
            line2
            */
            public int Count { get; set; }

        }

    }
}
", code, ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
    }
}

internal class ParentGeneric<T>
{
    public class Nested
    {
    }
}
