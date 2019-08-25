using System.Reflection;
using System.Runtime.CompilerServices;
using isukces.code.AutoCode;
using isukces.code.interfaces;
using Xunit;

namespace isukces.code.Tests.EqualityGenerator
{
    public partial class EqualityGeneratorTests
    {
        private static void CompareCode(string code,
            [CallerMemberName] string method = null, [CallerFilePath] string file = null)
        {
            TestUtils.CompareWithResource(code, "isukces.code.Tests.EqualityGenerator.", method, file, "_res.cs");
        }

        private static void DoTest<T>(
            [CallerMemberName] string method = null, [CallerFilePath] string file = null)
        {
            IMemberNullValueChecker c   = new MyValueChecker();
            var                     q   = new Generators.EqualityGenerator(c);
            var                     ctx = new TestContext();
            q.Generate(typeof(T), ctx);
            CompareCode(ctx.Code, method, file);
            // TestUtils.CompareWithResource(code, "isukces.code.Tests.EqualityGenerator.", method, file);
            //var s = (sbyte)1;
            // var cc = s.GetHashCode();
            //Assert.Equal(1,cc);
            // (int) (ushort) this | (int) this << 16
        }


        [Fact]
        public void T01_Should_create_basic_one_property_class()
        {
            DoTest<OnePropertyClass>();
        }

        [Fact]
        public void T02_Should_create_basic_one_property_struct()
        {
            DoTest<OnePropertyStruct>();
        }

        [Fact]
        public void T03_Should_create_equality_with_enums()
        {
            DoTest<ClassWithEnumProperties1>();
        }

        [Fact]
        public void T04_Should_create_equality_with_enums()
        {
            DoTest<ClassWithEnumProperties2>();
        }

        [Fact]
        public void T05_Should_create_equality_with_enums()
        {
            DoTest<ClassWithEnumProperties3>();
        }

        [Fact]
        public void T06_Should_create_equality_with_enums()
        {
            DoTest<ClassWithEnumProperties4>();
        }

        [Fact]
        public void T07_Should_create_equality_with_enums()
        {
            DoTest<ClassWithManyProperties>();
        }

        public enum EnumWithOffset
        {
            One = 5,
            Two = 8,
            Three = 12
        }

        public enum NormalEnum
        {
            One,
            Two,
            Three
        }


        [Auto.EqualityGeneratorAttribute]
        public partial struct OnePropertyStruct
        {
            public int IntValue { get; set; }
        }

        [Auto.EqualityGeneratorAttribute]
        public partial class OnePropertyClass
        {
            public int IntValue { get; set; }
        }

        [Auto.EqualityGeneratorAttribute]
        public partial class ClassWithEnumProperties1
        {
            public NormalEnum     N1 { get; set; }
            public EnumWithOffset O1 { get; set; }
            public NormalEnum     N2 { get; set; }
            public EnumWithOffset O2 { get; set; }
        }

        [Auto.EqualityGeneratorAttribute]
        public partial class ClassWithEnumProperties2
        {
            public int        IntValue { get; set; }
            public NormalEnum Normal   { get; set; }
        }

        [Auto.EqualityGeneratorAttribute]
        public partial class ClassWithEnumProperties3
        {
            public int            IntValue { get; set; }
            public EnumWithOffset Offset   { get; set; }
        }

        [Auto.EqualityGeneratorAttribute]
        public partial class ClassWithEnumProperties4
        {
            public EnumWithOffset Offset   { get; set; }
            public int            IntValue { get; set; }
        }

        [Auto.EqualityGeneratorAttribute]
        public partial class ClassWithManyProperties
        {
            public EnumWithOffset Offset       { get; set; }
            public int            IntValue     { get; set; }
            public uint           UIntValue    { get; set; }
            public short          ShortValue   { get; set; }
            public ushort         UShortValue  { get; set; }
            public decimal        DecimalValue { get; set; }

            public string NameNotNull { get; set; }

            [Auto.StringComparison.OrdinalIgnoreCaseAttribute]
            public string Id { get; set; }

            [Auto.StringComparison.OrdinalIgnoreCaseAttribute]
            public string IdNotNull { get; set; }
        }

        public class MyValueChecker : AbstractMemberNullValueChecker
        {
            protected override bool HasMemberNotNullAttribute(MemberInfo mi) => mi.Name.Contains("NotNull");
        }
    }
}