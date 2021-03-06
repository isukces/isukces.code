using System;
using iSukces.Code.CodeWrite;
using Xunit;

namespace iSukces.Code.Tests
{
    public class CsMethodTest
    {
        private static string TestCode(Action<CsClass> action)
        {
            var f  = new CsFile();
            var ns = f.GetOrCreateNamespace("My123");
            var cs = ns.GetOrCreateClass("MyClass");
            action(cs);
            var code = f.GetCode();
            return code;
        }

        [Fact]
        public void T01_Should_create_virtual_method()
        {
            const string exp = @"// ReSharper disable All
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
            var code = TestCode(cs =>
            {
                cs.AddMethod("Meth", (string)null)
                    .WithVirtual();
            });
            Assert.Equal(exp, code);
        }


        [Fact]
        public void T02_Should_create_abstract_method()
        {
            const string exp = @"// ReSharper disable All
namespace My123
{
    public abstract class MyClass
    {
        public abstract void Meth();

    }
}
";
            var code = TestCode(cs =>
            {
                cs.AddMethod("Meth", (string)null)
                    .WithAbstract();
            });
            Assert.Equal(exp, code);
        }


        [Fact]
        public void T03_Should_create_override_method()
        {
            const string exp = @"// ReSharper disable All
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
            var code = TestCode(cs =>
            {
                cs.AddMethod("Meth", (string)null)
                    .WithOverride();
            });
            Assert.Equal(exp, code);
        }


        [Fact]
        public void T04_Should_create_constructor()
        {
            const string exp = @"// ReSharper disable All
namespace My123
{
    public class MyClass
    {
        public MyClass()
        {
        }

    }
}
";
            var code = TestCode(cs => { cs.AddConstructor(); });
            Assert.Equal(exp, code);
        }

        [Theory]
        [InlineData(OverridingType.Abstract)]
        [InlineData(OverridingType.Virtual)]
        [InlineData(OverridingType.Override)]
        public void T05_Should_not_create_constructor_with_overriding_flags(OverridingType overriding)
        {
            Assert.Throws<Exception>(() =>
            {
                TestCode(cs =>
                {
                    var m = cs.AddConstructor();
                    m.Overriding = overriding;
                });
            });
        }


        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void T06_Should_create_finalizer(bool addExplicitNonStatic)
        {
            const string exp = @"// ReSharper disable All
namespace My123
{
    public class MyClass
    {
        ~MyClass()
        {
        }

    }
}
";
            var code = TestCode(cs =>
            {
                var m = cs.AddFinalizer();
                if (addExplicitNonStatic)
                    m.IsStatic = false;
            });
            Assert.Equal(exp, code);
        }

        [Fact]
        public void T07_Should_not_create_finalizer_static()
        {
            Assert.Throws<Exception>(() =>
            {
                TestCode(cs =>
                {
                    var m = cs.AddFinalizer();
                    m.IsStatic = true;
                });
            });
        }

        [Theory]
        [InlineData(OverridingType.Abstract)]
        [InlineData(OverridingType.Virtual)]
        [InlineData(OverridingType.Override)]
        public void T08_Should_not_create_finalizer_with_overriding_flags(OverridingType overriding)
        {
            var f  = new CsFile();
            var ns = f.GetOrCreateNamespace("My123");
            var cs = ns.GetOrCreateClass("MyClass");
            cs.AddConstructor().Overriding = overriding;
            Assert.Throws<Exception>(() => { f.GetCode(); });
        }

        [Fact]
        public void T09_Should_binary_operator()
        {
            const string exp = @"// ReSharper disable All
namespace My123
{
    public class MyClass
    {
        public static Bla operator +(Bla left, Bla rigth)
        {
            return new Bla(left.Value + right.Value);
        }

    }
}
";
            var code = TestCode(cs =>
            {
                var m = cs.AddMethod("+", "Bla");
                m.AddParam("left", "Bla");
                m.AddParam("rigth", "Bla");
                m.WithBody("return new Bla(left.Value + right.Value);");
            });
            Assert.Equal(exp, code);
        }
    }
}