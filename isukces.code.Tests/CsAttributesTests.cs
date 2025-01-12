#nullable disable
using System;
using iSukces.Code.Interfaces;
using Xunit;

namespace iSukces.Code.Tests;

public class CsAttributesTests
{
    [Fact]
    public void T01_Should_Create_attribute_with_debug()
    {
        var f = new CsFile();
        var ns = f.GetOrCreateNamespace("Tests");

        var cl = new CsClass((CsType)"Demo") {Kind = CsNamespaceMemberKind.Class, Owner = ns};
        cl.Attributes.Add(new CsAttribute("SampleAttribute")
        {
            CompilerDirective = "DEBUG1"
        });
        {
            var ev = cl.AddEvent<EventHandler<EventArgs>>("Sample", "Description");
            ev.Attributes.Add(new CsAttribute("SampleAttribute")
            {
                CompilerDirective = "DEBUG2"
            });

        }
        {
            var ev = cl.AddMethod("SampleMethod", CsType.String);
            ev.Attributes.Add(new CsAttribute("SampleAttribute")
            {
                CompilerDirective = "DEBUG3"
            });

        }       
        {
            var ev = cl.AddProperty("Property", CsType.String);
            ev.Attributes.Add(new CsAttribute("SampleAttribute")
            {
                CompilerDirective = "DEBUG4"
            });

        }

        ICsCodeWriter w = new CsCodeWriter();
        cl.MakeCode(w);
        var expected = @"
#if DEBUG1
[Sample]
#endif
public class Demo
{
#if DEBUG3
    [Sample]
#endif
    public string SampleMethod()
    {
    }

#if DEBUG4
    [Sample]
#endif
    public string Property { get; set; }

    /// <summary>
    /// Description
    /// </summary>
#if DEBUG2
    [Sample]
#endif
    public event System.EventHandler<System.EventArgs> Sample;
}

";
        Assert.Equal(expected.Trim(), w.Code.Trim());
    }
}
