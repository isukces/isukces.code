#nullable enable
using System.Collections.Generic;
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
}
