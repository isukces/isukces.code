#nullable enable
using System.Collections.Generic;
using iSukces.Code.Interfaces;
using Xunit;

namespace iSukces.Code.Tests;

public sealed class CsNamespaceTests
{
    [Fact]
    public void T01_Should_convert_int()
    {
        var file = new CsFile();
        var ns   = file.GetOrCreateNamespace("iSukces.Code");
        ns.AddImportNamespace<CsNamespaceTests>();
        file.AddImportNamespace<CsClass>();
        file.FileScopeNamespace = FileScopeNamespaceConfiguration.AllowIfPossible;

        var code = file.GetCode().Trim();
        const string expected = @"// ReSharper disable All
using iSukces.Code;

namespace iSukces.Code;

using iSukces.Code.Tests;";
        Assert.Equal(expected, code);
    }
    
    [Fact]
    public void T02_Should_convert_with_alias()
    {
        var file = new CsFile();
        var ns   = file.GetOrCreateNamespace("iSukces.Code");
        ns.AddImportNamespace<CsNamespaceTests>("tests");
        ns.AddImportNamespace<System.Globalization.Calendar>();
        ns.Usings.AddTypeAlias("TStringList", typeof(List<string>));

        file.AddImportNamespace<CsClass>();
        file.AddImportNamespace<List<int>>("gene");
        file.FileScopeNamespace = FileScopeNamespaceConfiguration.AllowIfPossible;
        {
            var cl =ns.GetOrCreateClass("SomeClass");
            cl.AddProperty<int>("A");
            cl.AddProperty<CsClass>("B");
            cl.AddProperty<List<int>>("C");
            cl.AddProperty<System.Globalization.Calendar>("D");
            cl.AddProperty<List<string>>("Sl");
        }

        var code = file.GetCode().Trim();
        const string expected = @"
// ReSharper disable All
using iSukces.Code;
using gene = System.Collections.Generic;

namespace iSukces.Code;

using System.Globalization;
using tests = iSukces.Code.Tests;
using TStringList = System.Collections.Generic.List<string>;

public class SomeClass
{
    public int A { get; set; }

    public CsClass B { get; set; }

    public gene.List<int> C { get; set; }

    public Calendar D { get; set; }

    public TStringList Sl { get; set; }

}
";
        Assert.Equal(expected.Trim(), code);
    }
}
