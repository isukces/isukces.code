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
            Assert.Equal(quote + backslash + "r" + quote, specialR.CsCite());
            var specialN = "\n";
            Assert.Equal(1,                               specialN.Length);
            Assert.Equal(quote + backslash + "n" + quote, specialN.CsCite());
            var specialT = "\t";
            Assert.Equal(1,                               specialT.Length);
            Assert.Equal(quote + backslash + "t" + quote, specialT.CsCite());
        }

        [Fact]
        public void T02_Should_Create_operator()
        {
            var cl = new CsClass("Src1");
            cl.Kind = NamespaceMemberKind.Struct;
            var m  = cl.AddMethod("*", "Result")
                .WithBody("return new Result(left.Value * right.Value);");
            m.AddParam("left",  "Src1");
            m.AddParam("right", "Src2");
            // odwrotny

            ICodeWriter w = new CodeWriter();
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

            ICodeWriter w = new CodeWriter();
            cl.MakeCode(w);
            var expected = @"public class Src1
{
    public int A { get; set; }

}
";
            Assert.Equal(expected.Trim(), w.Code.Trim());

            p.ConstValue = "12";
            w = new CodeWriter();
            cl.MakeCode(w);
              expected = @"public class Src1
{
    public int A { get; set; } = 12;

}
";
            Assert.Equal(expected.Trim(), w.Code.Trim());
        }
    }
}