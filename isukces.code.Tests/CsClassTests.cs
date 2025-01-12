#nullable disable
using iSukces.Code.Interfaces;
using Xunit;

namespace iSukces.Code.Tests;

public sealed class CsClassTests
{
    public static string Encode(string c)
    {
        c = c.Replace("\"", "\"\"");
        c = "@\"" + c + "\"";
        return c;
    }

    [Fact]
    public void T01_Should_create_class_with_primary_constructor()
    {
        var code = CsMethodTest.TestCode(cs =>
        {
            cs.PrimaryConstructor = new CsPrimaryConstructor(new CsMethodParameter("a", CsType.Int32));
        });
        
        
        const string exp = @"// ReSharper disable All
// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace My123
{
    public class MyClass(int a)
    {
    }
}

";
        Assert.Equal(exp.Trim(), code.Trim());
        
    }
    
    [Fact]
    public void T02_Should_create_record()
    {
        var code = CsMethodTest.TestCode(cs =>
        {
            cs.PrimaryConstructor = new CsPrimaryConstructor(new CsMethodParameter("a", CsType.Int32));
            cs.Kind               = CsNamespaceMemberKind.Record;
        });
        
        
        const string exp = @"// ReSharper disable All
// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace My123
{
    public record MyClass(int a);
}

";
        Assert.Equal(exp.Trim(), code.Trim());
        
    }
    
    
    [Fact]
    public void T03_Should_create_record_struct()
    {
        var code = CsMethodTest.TestCode(cs =>
        {
            cs.PrimaryConstructor = new CsPrimaryConstructor(new CsMethodParameter("a", CsType.Int32));
            cs.Kind               = CsNamespaceMemberKind.RecordStruct;
        });
        
        
        const string exp = @"// ReSharper disable All
// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace My123
{
    public record struct MyClass(int a);
}

";
        Assert.Equal(exp.Trim(), code.Trim());
        
    }
}
