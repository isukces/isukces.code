using System;
using System.ComponentModel;
using System.Linq;
using iSukces.Code.CodeWrite;
using iSukces.Code.Interfaces;
using Xunit;

namespace iSukces.Code.Tests
{
    public class CsEnumTests
    {
        [Fact]
        public void T01_Should_create_enum()
        {
            var f  = new CsFile();
            var ns = f.GetOrCreateNamespace("My123");
            var enu = new CsEnum("MyEnum",
                new CsEnumItem("Jeden"),
                new CsEnumItem("Dwa"));
            ns.Enums.Add(enu);

            var code = f.GetCode();
            var exp = @"// ReSharper disable All
namespace My123
{
    public enum MyEnum
    {
        Jeden,
        Dwa
    }
}
";
            Assert.Equal(exp, code);
        }

        [Fact]
        public void T02_Should_create_enum_flag()
        {
            var f  = new CsFile();
            var ns = f.GetOrCreateNamespace("My123");
            var enu = new CsEnum("MyEnum",
                new CsEnumItem("Jeden", 1),
                new CsEnumItem("Dwa", 2));
            enu.WithAttribute(CsAttribute.Make<FlagsAttribute>(ns));


            enu.Items.Last().WithAttribute(CsAttribute.Make<DescriptionAttribute>(ns).WithArgument("kuku"));
            
            ns.Enums.Add(enu);

            var code = f.GetCode();
            var exp = @"// ReSharper disable All
namespace My123
{
    [System.Flags]
    public enum MyEnum
    {
        Jeden = 1,
        [System.ComponentModel.Description(""kuku"")]
        Dwa = 2
    }
}
";
            var code2 = $"var exp = {code.CsVerbatimEncode()};";
            Assert.Equal(exp, code);
        }
    }
}