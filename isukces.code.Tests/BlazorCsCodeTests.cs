using System;
using System.Globalization;
using iSukces.Code.Interfaces;
using Xunit;
using Xunit.Abstractions;

namespace iSukces.Code.Tests;

public class BlazorCsCodeTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public BlazorCsCodeTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void T01_Should_Create_wrapped_blazor_code()
    {
        var f = new CsFile();
        var ns = f.GetOrCreateNamespace("Tests");
        f.AddImportNamespace("System.Globalization");
        f.AddImportNamespace("System.Collections.Generic");
        var cl = ns.GetOrCreateClass("Demo");
        cl.AddProperty("PropertyGuid", typeof(Guid));
        cl.AddProperty("PropertyCultureInfo", typeof(CultureInfo));


        ICsCodeWriter w = new CsCodeWriter();
        cl.MakeCodeForBlazor(w, default, true);
        var expected = @"

@code {
    public System.Guid PropertyGuid { get; set; }

    public CultureInfo PropertyCultureInfo { get; set; }

}
";
        _testOutputHelper.WriteLine(w.Code);
        Assert.Equal(expected.Trim(), w.Code.Trim());
    }

}