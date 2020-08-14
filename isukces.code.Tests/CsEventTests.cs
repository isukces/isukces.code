using System;
using isukces.code.CodeWrite;
using isukces.code.interfaces;
using Xunit;

namespace isukces.code.Tests
{
    public class CsEventTests
    {
        [Fact]
        public void T01_Should_Create_event()
        {
            var f = new CsFile();
            var ns = f.GetOrCreateNamespace("Tests");

            var cl = new CsClass("Demo") {Kind = CsNamespaceMemberKind.Class, Owner = ns};
            var ev = cl.AddEvent<EventHandler<EventArgs>>("Sample", "Description");
            ev.Attributes.Add(new CsAttribute("SampleAttribute"));
            
            ICsCodeWriter w = new CsCodeWriter();
            cl.MakeCode(w);
            var expected = @"
public class Demo
{
    /// <summary>
    /// Description
    /// </summary>
    [Sample]
    public event System.EventHandler<System.EventArgs> Sample;
}

";
            Assert.Equal(expected.Trim(), w.Code.Trim());
        }
    }
}