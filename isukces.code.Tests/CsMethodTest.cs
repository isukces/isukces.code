using iSukces.Code.CodeWrite;
using Xunit;

namespace iSukces.Code.Tests
{
    public class CsMethodTest
    {
        [Fact]
        public void T01_Should_create_virtual_method()
        {
            var f  = new CsFile();
            var ns = f.GetOrCreateNamespace("My123");
            var cs = ns.GetOrCreateClass("MyClass");
            cs.AddMethod("Meth", (string)null)
                .WithVirtual();
            var code = f.GetCode();
            var exp = @"// ReSharper disable All
namespace My123
{
    public class MyClass
    {
        public virtual void Meth()
        {
        }

    }
}
";
            Assert.Equal(exp, code);
        }
        
        
        [Fact]
        public void T02_Should_create_abstract_method()
        {
            var f  = new CsFile();
            var ns = f.GetOrCreateNamespace("My123");
            var cs = ns.GetOrCreateClass("MyClass");
            cs.AddMethod("Meth", (string)null)
                .WithAbstract();
            var code = f.GetCode();
            var exp = @"// ReSharper disable All
namespace My123
{
    public abstract class MyClass
    {
        public abstract void Meth();

    }
}
";
            Assert.Equal(exp, code);
        }
        
        
        [Fact]
        public void T03_Should_create_override_method()
        {
            var f  = new CsFile();
            var ns = f.GetOrCreateNamespace("My123");
            var cs = ns.GetOrCreateClass("MyClass");
            cs.AddMethod("Meth", (string)null)
                .WithOverride();
            var code = f.GetCode();
            var exp = @"// ReSharper disable All
namespace My123
{
    public class MyClass
    {
        public override void Meth()
        {
        }

    }
}
";
            Assert.Equal(exp, code);
        }
    }
}