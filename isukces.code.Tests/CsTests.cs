using iSukces.Code;
using iSukces.Code.Interfaces;
using Xunit;

namespace iSukces.Code.Tests
{
    public class CsTests
    {
        [Fact]
        public void T01_ShouldCsCite()
        {
            const string quote     = "\"";
            const string backslash = "\\";

            const string specialR = "\r";
            Assert.Equal(1, specialR.Length);
            Assert.Equal(quote + backslash + "r" + quote, specialR.CsEncode());
            var specialN = "\n";
            Assert.Equal(1, specialN.Length);
            Assert.Equal(quote + backslash + "n" + quote, specialN.CsEncode());
            var specialT = "\t";
            Assert.Equal(1, specialT.Length);
            Assert.Equal(quote + backslash + "t" + quote, specialT.CsEncode());
        }

        [Fact]
        public void T02_Should_Create_operator()
        {
            var cl = new CsClass((CsType)"Src1");
            cl.Kind = CsNamespaceMemberKind.Struct;
            cl.AddMethod("*", (CsType)"Result")
                .WithBodyAsExpression("new Result(left.Value * right.Value)")
                .WithParameter("left", (CsType)"Src1")
                .WithParameter("right", (CsType)"Src2");
            // odwrotny

            ICsCodeWriter w = new CsCodeWriter();
            cl.MakeCode(w);
            var expected = @"public struct Src1
{
    public static Result operator *(Src1 left, Src2 right)
    {
        return new Result(left.Value * right.Value);
    }

}
";
            Assert.Equal(expected.Trim(), w.Code.Trim());
        }


        [Fact]
        public void T03_Should_Create_auto_property_with_initialisation()
        {
            var cl = new CsClass((CsType)"Src1");
            var p  = cl.AddProperty("A", CsType.Int32);
            p.MakeAutoImplementIfPossible = true;
            //p.ConstValue = "12";
            // odwrotny

            var w = new CsCodeWriter();
            cl.MakeCode(w);
            var expected = @"public class Src1
{
    public int A { get; set; }

}
";
            Assert.Equal(expected.Trim(), w.GetCodeTrim());

            p.ConstValue = "12";
            w            = new CsCodeWriter();
            cl.MakeCode(w);
            expected = @"public class Src1
{
    public int A { get; set; } = 12;

}
";
            Assert.Equal(expected.Trim(), w.GetCodeTrim());
        }

        [Fact]
        public void T04_Should_Create_interface()
        {
            var cl = new CsClass((CsType)"ITest")
            {
                Kind = CsNamespaceMemberKind.Interface
            };
            var m = cl.AddMethod("Count", CsType.Int32)
                .WithBody("return 12;");

            var p = cl.AddProperty("A", CsType.Int32);
            p.MakeAutoImplementIfPossible = true;
            p.OwnGetter                   = "return 123;";
            //p.ConstValue = "12";
            // odwrotny

            var w = new CsCodeWriter();
            cl.MakeCode(w);
            var expected = @"
public interface ITest
{
    int Count();

    int A { get; set; }

}";

            Assert.Equal(expected.Trim(), w.GetCodeTrim());

            p.ConstValue = "12";
            w            = new CsCodeWriter();
            cl.MakeCode(w);

            Assert.Equal(expected.Trim(), w.GetCodeTrim());
        }


        [Fact]
        public void T05_Should_generate_compiler_directive()
        {
            var cl = new CsClass((CsType)"Src1");
            var p  = cl.AddProperty("A", CsType.Int32);
            p.MakeAutoImplementIfPossible = true;
            cl.CompilerDirective          = "DEBUG";

            var w = new CsCodeWriter();
            cl.MakeCode(w);
            var expected = @"
#if DEBUG
public class Src1
{
    public int A { get; set; }

}
#endif

";
            Assert.Equal(expected.Trim(), w.GetCodeTrim());
        }

        [Fact]
        public void T06_Should_cut_namespace()
        {
            var f = new CsFile();
            f.AddImportNamespace("System.Alpha");
            var ns = f.GetOrCreateNamespace("Custom.Beta");
            ns.AddImportNamespace("Custom.Private");
            var c = ns.GetOrCreateClass((CsType)"Gamma");

            Assert.True(c.IsKnownNamespace("Custom.Beta"));
            Assert.True(c.IsKnownNamespace("System.Alpha"));
            Assert.True(c.IsKnownNamespace("Custom.Private"));
            Assert.False(c.IsKnownNamespace("Some.Unknown.Namespace"));

            Assert.True(ns.IsKnownNamespace("Custom.Beta"));
            Assert.True(ns.IsKnownNamespace("System.Alpha"));
            Assert.True(ns.IsKnownNamespace("Custom.Private"));
            Assert.False(ns.IsKnownNamespace("Some.Unknown.Namespace"));

            Assert.False(f.IsKnownNamespace("Custom.Beta"));
            Assert.True(f.IsKnownNamespace("System.Alpha"));
            Assert.False(f.IsKnownNamespace("Custom.Private"));
            Assert.False(f.IsKnownNamespace("Some.Unknown.Namespace"));

            var w = new CsCodeWriter();
            f.MakeCode(w);
            const string expected = @"// ReSharper disable All
using System.Alpha;

// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace Custom.Beta
{
    using Custom.Private;

    public class Gamma
    {
    }
}
";
            Assert.Equal(expected.Trim(), w.Code.Trim());
        }
    }
}