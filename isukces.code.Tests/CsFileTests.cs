using iSukces.Code.Interfaces;
using Xunit;

namespace iSukces.Code.Tests;

public sealed class CsFileTests
{
    public static string Encode(string c)
    {
        c = c.Replace("\"", "\"\"");
        c = "@\"" + c + "\"";
        return c;
    }

    [Fact]
    public void T01_ShouldAddNullableEnabled()
    {
        var file = new CsFile
        {
            Nullable = FileNullableOption.LocalEnabled
        };
        file.GetOrCreateClass("Bla", (CsType)"ClassName");
        ICsCodeWriter w = new CsCodeWriter();
        file.MakeCode(w);
        var newExpected = Encode(w.Code);
        var expected = @"
#nullable enable
// ReSharper disable All
// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace Bla
{
    public class ClassName
    {
    }
}
";
        Assert.Equal(expected.Trim(), w.Code.Trim());
    }
    
    
    [Fact]
    public void T02_ShouldAddNullableDisabled()
    {
        var file = new CsFile
        {
            Nullable = FileNullableOption.LocalDisabled
        };
        file.GetOrCreateClass("Bla", (CsType)"ClassName");
        ICsCodeWriter w = new CsCodeWriter();
        file.MakeCode(w);
        var newExpected = Encode(w.Code);
        var expected = @"
#nullable disable
// ReSharper disable All
// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace Bla
{
    public class ClassName
    {
    }
}
";
        Assert.Equal(expected.Trim(), w.Code.Trim());
    }
}