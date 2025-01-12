#nullable disable
using iSukces.Code;
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
            gen.Flags = DependsOnPropertyGeneratorFlags.None;

            var q = new TestContext();
            gen.Generate(typeof(Test), q);
            const string exp = @"// ReSharper disable All
// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
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
                Flags                                  = DependsOnPropertyGeneratorFlags.GetDependentProperties,
                GetDependentPropertiesMethodName       = "TestGetDependentProperties",
                GetDependentPropertiesMethodVisibility = Visibilities.Protected
            };

            var q = new TestContext();
            gen.Generate(typeof(Test), q);
            const string exp = @"
// ReSharper disable All
// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace iSukces.Code.Tests.DependsOnPropertyGenerator
{
    partial class DependsOnPropertyGeneratorTests
    {
        partial class Test
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            private static string TestGetDependentProperties(string propertyName)
            {
                // generator : DependsOnPropertyGenerator.MakeGetDependentProperties:122
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
            Assert.Equal(exp.Trim(), q.Code.Trim());
        }
        
        
        [Fact]
        public void T03_Should_generate_with_GetDependedntProperties()
        {
            var gen = new Code.DependsOnPropertyGenerator
            {
                Flags                                  = DependsOnPropertyGeneratorFlags.GetDependentProperties,
                GetDependentPropertiesMethodName       = "TestGetDependentProperties",
                GetDependentPropertiesMethodVisibility = Visibilities.Protected
            };

            var q = new TestContext();
            gen.Generate(typeof(TestCascade), q);
         
          const string expected = @"
// ReSharper disable All
// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace iSukces.Code.Tests.DependsOnPropertyGenerator
{
    partial class DependsOnPropertyGeneratorTests
    {
        partial class TestCascade
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            private static string TestGetDependentProperties(string propertyName)
            {
                // generator : DependsOnPropertyGenerator.MakeGetDependentProperties:122
                switch (propertyName)
                {
                    case nameof(Master): return MasterDependent; // Slave,DeepSlave,EvenDeeperSlave
                    case nameof(Slave): return ""DeepSlave,EvenDeeperSlave"";
                    case nameof(DeepSlave): return DeepSlaveDependent; // EvenDeeperSlave
                    case nameof(FirstName):
                    case nameof(LastName): return LastNameDependent; // FullNameA,FullNameB,FullNameA1,FullNameB1
                    case nameof(FullNameA): return ""FullNameA1"";
                }
                return null;
            }

            public const string MasterDependent = ""Slave,DeepSlave,EvenDeeperSlave"";

            public const string DeepSlaveDependent = ""EvenDeeperSlave"";

            public const string FirstNameDependent = ""FullNameA,FullNameB,FullNameA1,FullNameB1"";

            public const string LastNameDependent = FirstNameDependent;

        }

    }
}
";
          var code = q.Code.Trim();
          Assert.Equal(expected.Trim(), code);
        }
        
        
           [Fact]
        public void T04_Should_not_allow_to_exclude_master_property()
        {
            var gen = new Code.DependsOnPropertyGenerator
            {
                Flags                                  = DependsOnPropertyGeneratorFlags.GetDependentProperties,
                GetDependentPropertiesMethodName       = "TestGetDependentProperties",
                GetDependentPropertiesMethodVisibility = Visibilities.Protected
            };

            var q = new TestContext();
            Assert.Throws<DependsOnPropertyGeneratorException>(() =>
            {
                gen.Generate(typeof(WrongClass), q);
            });
        }
        
        
        private class Test
        {
            public int Master { get; set; }

            [DependsOnProperty(nameof(Master))]
            public int Slave => Master + 1;
        }
        
        
        [DependsOnProperty2(SkipCreatingConstants = true)]
        private class TestCascade
        {
            public int Master { get; set; }

            [DependsOnProperty(nameof(Master), Flags = DependsOnPropertyFlags.SkipCreatingConstants)]
            public int Slave => Master + 1;
            
            [DependsOnProperty(nameof(Slave))]
            public int DeepSlave => Slave + 1;
            
            
            [DependsOnProperty(nameof(DeepSlave))]
            public int EvenDeeperSlave => DeepSlave + 1;
            
            
            public string FirstName { get; set; }
            public string LastName  { get; set; }
            
            [DependsOnProperty(nameof(FirstName), nameof(LastName), Flags = DependsOnPropertyFlags.SkipCreatingConstants)]
            public string FullNameA  => FirstName + " " + LastName;
            
            
            
            [DependsOnProperty(nameof(FirstName), nameof(LastName), Flags = DependsOnPropertyFlags.SkipCreatingConstants|DependsOnPropertyFlags.ExcludeFromGetDependentPropertiesMethod)]
            public string FullNameB => FirstName + " " + LastName;





            [DependsOnProperty(nameof(FullNameA),  Flags = DependsOnPropertyFlags.SkipCreatingConstants)]
            public string FullNameA1 => FullNameA + "?";
            
            [DependsOnProperty(nameof(FullNameB),  Flags = DependsOnPropertyFlags.ExcludeFromGetDependentPropertiesMethod)]
            public string FullNameB1 => FullNameB + "?";

        }
    }


    public class WrongClass
    {
        [DependsOnProperty(Flags = DependsOnPropertyFlags.ExcludeFromGetDependentPropertiesMethod)]
        public string FirstName { get; set; }
        public string LastName  { get; set; }
            
        
        [DependsOnProperty(nameof(FirstName), nameof(LastName))]
        public string FullName => FirstName + " " + LastName;
    }

}
