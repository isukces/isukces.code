using isukces.code.CodeWrite;
using isukces.code.interfaces;
using Xunit;

namespace isukces.code.Tests
{
    public class CsTests
    {
        [Fact]
        public void ShouldCsCite()
        {
            const string quote     = "\"";
            const string backslash = "\\";

            const string specialR = "\r";
            Assert.Equal(1,                               specialR.Length);
            Assert.Equal(quote + backslash + "r" + quote, specialR.CsEncode());
            var specialN = "\n";
            Assert.Equal(1,                               specialN.Length);
            Assert.Equal(quote + backslash + "n" + quote, specialN.CsEncode());
            var specialT = "\t";
            Assert.Equal(1,                               specialT.Length);
            Assert.Equal(quote + backslash + "t" + quote, specialT.CsEncode());
        }

        [Fact]
        public void T02_Should_Create_operator()
        {
            var cl = new CsClass("Src1");
            cl.Kind = CsNamespaceMemberKind.Struct;
            var m  = cl.AddMethod("*", "Result")
                .WithBody("return new Result(left.Value * right.Value);");
            m.AddParam("left",  "Src1");
            m.AddParam("right", "Src2");
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
            var cl = new CsClass("Src1");
            var p = cl.AddProperty("A", "int");
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
            w = new CsCodeWriter();
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
            var cl = new CsClass("ITest")
            {
                Kind = CsNamespaceMemberKind.Interface
            };
            var m = cl.AddMethod("Count", "int")
                .WithBody("return 12;");
            
            var p  = cl.AddProperty("A", "int");
            p.MakeAutoImplementIfPossible = true;
            p.OwnGetter = "return 123;";
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
            var cl = new CsClass("Src1");
            var p  = cl.AddProperty("A", "int");
            p.MakeAutoImplementIfPossible = true;
            cl.CompilerDirective = "DEBUG";

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

    }
}