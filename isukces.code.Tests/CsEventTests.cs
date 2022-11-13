using System;
using iSukces.Code.Interfaces;
using Xunit;

namespace iSukces.Code.Tests
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
        
        
        /// <summary>
        /// aaa <see cref="Sampele">Sample</see>
        /// </summary>
        [Fact]
        public void T02_Should_Create_event_with_long_definition()
        {
            var f  = new CsFile();
            var ns = f.GetOrCreateNamespace("Tests");

            var cl = new CsClass("Demo") {Kind = CsNamespaceMemberKind.Class, Owner = ns};
            var ev = cl.AddEvent<EventHandler<EventArgs>>("Sample", "Description");
            ev.LongDefinition = true;
            ev.Attributes.Add(new CsAttribute("SampleAttribute"));
            
            ICsCodeWriter w = new CsCodeWriter();
            cl.MakeCode(w);
            var expected = @"
public class Demo
{
    /// <summary>
    /// field for Sample event
    /// </summary>
    public System.EventHandler<System.EventArgs> _sample;

    /// <summary>
    /// Description
    /// </summary>
    [Sample]
    public event System.EventHandler<System.EventArgs> Sample;
    {
        add { _sample += value; }
        remove { _sample -= value; }
    }
}


";
            Assert.Equal(expected.Trim(), w.Code.Trim());
        }
    }
}