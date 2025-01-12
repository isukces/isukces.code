#nullable disable
using System;
using System.Globalization;
using iSukces.Code.Interfaces;
using Xunit;

namespace iSukces.Code.Tests;

public class GlobalNamespacesTests
{
    [Fact]
    public void T01_Should_Create_class_without_global_usings()
    {
        var f = new CsFile();
        var ns = f.GetOrCreateNamespace("Tests");
        f.AddImportNamespace("System.Globalization");
        f.AddImportNamespace("System.Collections.Generic");
        var cl = ns.GetOrCreateClass("Demo");
        cl.AddProperty("PropertyGuid", typeof(Guid));
        cl.AddProperty("PropertyCultureInfo", typeof(CultureInfo));


        ICsCodeWriter w = new CsCodeWriter();
        f.MakeCode(w);
        var expected = @"

// ReSharper disable All
using System.Collections.Generic;
using System.Globalization;

// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace Tests
{
    public class Demo
    {
        public System.Guid PropertyGuid { get; set; }

        public CultureInfo PropertyCultureInfo { get; set; }

    }
}

";
        Assert.Equal(expected.Trim(), w.Code.Trim());
    }

    [Fact]
    public void T02_Should_Create_class_with_global_usings()
    {
        var f = new CsFile
        {
            GlobalUsings = new GlobalUsingsConfiguration().WithStandard()
        };
        var ns = f.GetOrCreateNamespace("Tests");
        f.AddImportNamespace("System.Globalization");
        f.AddImportNamespace("System.Collections.Generic");
        var cl = ns.GetOrCreateClass("Demo");
        cl.AddProperty("PropertyGuid", typeof(Guid));
        cl.AddProperty("PropertyCultureInfo", typeof(CultureInfo));
        
        ICsCodeWriter w = new CsCodeWriter();
        f.MakeCode(w);
        var expected = @"

// ReSharper disable All
using System.Globalization;

// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace Tests
{
    public class Demo
    {
        public Guid PropertyGuid { get; set; }

        public CultureInfo PropertyCultureInfo { get; set; }

    }
}

";
        Assert.Equal(expected.Trim(), w.Code.Trim());
    }
}
