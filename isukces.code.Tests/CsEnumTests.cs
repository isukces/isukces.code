#nullable disable
using System;
using System.ComponentModel;
using System.Linq;
using iSukces.Code;
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
            ns.AddEnum(enu);

            var code = f.GetCode();
            var exp = @"// ReSharper disable All
// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
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
            
            ns.AddEnum(enu);

            var code = f.GetCode();
            var exp = @"// ReSharper disable All
// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
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
        
        [Fact]
        public void T03_Should_create_enum_with_description()
        {
            var f  = new CsFile();
            var ns = f.GetOrCreateNamespace("My123");
            var enu = new CsEnum("MyEnum",
                new CsEnumItem("Jeden", 1) {Description = "desc 1"},
                new CsEnumItem("Dwa", 2) {Description   = "desc 2"}
            )
            {
                Description = "enum desc"
            };
            
            enu.Items.Last().WithAttribute(CsAttribute.Make<DescriptionAttribute>(ns).WithArgument("kuku"));
            
            ns.AddEnum(enu);

            var code = f.GetCode();
            var exp = @"// ReSharper disable All
// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace My123
{
    /// <summary>
    /// enum desc
    /// </summary>
    public enum MyEnum
    {
        /// <summary>
        /// desc 1
        /// </summary>
        Jeden = 1,
        /// <summary>
        /// desc 2
        /// </summary>
        [System.ComponentModel.Description(""kuku"")]
        Dwa = 2
    }
}
";
            var code2 = $"var exp = {code.CsVerbatimEncode()};";
            Assert.Equal(exp, code);
        }
        
        [Fact]
        public void T04_Should_create_nested_enum()
        {
            var f  = new CsFile();
            var ns = f.GetOrCreateNamespace("My123");
         
            var enu = new CsEnum("MyEnum",
                new CsEnumItem("Jeden", 1) {Description = "desc 1"},
                new CsEnumItem("Dwa", 2) {Description   = "desc 2"}
            )
            {
                Description = "enum desc"
            };
            
            enu.Items.Last().WithAttribute(CsAttribute.Make<DescriptionAttribute>(ns).WithArgument("kuku"));
            
            var cl = ns.GetOrCreateClass((CsType)"OwnerClass");
            cl.AddEnum(enu);

            var code = f.GetCode();
            var exp = @"// ReSharper disable All
// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace My123
{
    public class OwnerClass
    {
        /// <summary>
        /// enum desc
        /// </summary>
        public enum MyEnum
        {
            /// <summary>
            /// desc 1
            /// </summary>
            Jeden = 1,
            /// <summary>
            /// desc 2
            /// </summary>
            [System.ComponentModel.Description(""kuku"")]
            Dwa = 2
        }

    }
}
";
            var code2 = ToCode(code);
            Assert.Equal(exp, code);
        }

        private static string ToCode(string code) => $"const string exp = {code.CsVerbatimEncode()};";
    }
}
