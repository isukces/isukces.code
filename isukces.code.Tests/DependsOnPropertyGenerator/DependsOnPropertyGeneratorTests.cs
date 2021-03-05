using iSukces.Code.Interfaces;
using Xunit;

namespace iSukces.Code.Tests.DependsOnPropertyGenerator
{
    public class DependsOnPropertyGeneratorTests
    {
        [Fact]
        public void T01_Should_generate_simple()
        {
            var gen = new Code.DependsOnPropertyGenerator();
            gen.Flags = DependsOnPropertyGeneratorF.None;

            var q = new TestContext();
            gen.Generate(typeof(Test), q);
            const string exp = @"// ReSharper disable All
namespace iSukces.Code.Tests.DependsOnPropertyGenerator
{
    partial class DependsOnPropertyGeneratorTests
    {
        partial class Test
        {
            public const string MasterDependent = ""Slave"";

        }

    }
}
";
            Assert.Equal(exp, q.Code);
        }


        [Fact]
        public void T02_Should_generate_with_GetDependedntProperties()
        {
            var gen = new Code.DependsOnPropertyGenerator
            {
                Flags                                  = DependsOnPropertyGeneratorF.GetDependentProperties,
                GetDependentPropertiesMethodName       = "TestGetDependentProperties",
                GetDependentPropertiesMethodVisibility = Visibilities.Protected
            };

            var q = new TestContext();
            gen.Generate(typeof(Test), q);
            const string exp = @"// ReSharper disable All
namespace iSukces.Code.Tests.DependsOnPropertyGenerator
{
    partial class DependsOnPropertyGeneratorTests
    {
        partial class Test
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            private static string TestGetDependentProperties(string propertyName)
            {
                switch (propertyName)
                {
                    case nameof(Master): return MasterDependent; // Slave
                }
                return null;
            }

            public const string MasterDependent = ""Slave"";

        }

    }
}
";
            Assert.Equal(exp, q.Code);
        }
        
        private class Test
        {
            public int Master { get; set; }

            [DependsOnProperty(nameof(Master))]
            public int Slave => Master + 1;
        }
    }
}