using System.Reflection;
using System.Runtime.CompilerServices;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;
using Xunit;

namespace iSukces.Code.Tests.EqualityGenerator;

public partial class EqualityGeneratorTests
{
    private static void CompareCode(string code,
        [CallerMemberName] string? method = null, [CallerFilePath] string? file = null)
    {
        TestUtils.CompareWithResource(code, "iSukces.Code.Tests.EqualityGenerator.", method, file, "_res.cs");
    }

    private static void DoTest<T>(
        [CallerMemberName] string? method = null, [CallerFilePath] string? file = null)
    {
        IMemberNullValueChecker c   = new MyValueChecker();
        var                     q   = new Generators.EqualityGenerator(c);
        var            ctx = new TestContext();
        q.Generate(typeof(T), ctx);
        CompareCode(ctx.Code, method, file);
        // TestUtils.CompareWithResource(code, "iSukces.Code.Tests.EqualityGenerator.", method, file);
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
        
    [Fact]
    public void T08_Should_create_equality_with_nullables()
    {
        DoTest<ClassWithNullable>();
        var a = new ClassWithNullable();
        Assert.Equal(0, a.GetHashCode2());
    }
        
    [Fact]
    public void T09_Should_create_equality_with_bools()
    {
        DoTest<ClassWithBools>();
    }
        
        
    [Fact]
    public void T10_Should_create_get_hash_code_with_special_hashCode_fields()
    {
        DoTest<TestStructWithSpecialHashCodeField>();
    }


    public class MyValueChecker : AbstractMemberNullValueChecker
    {
        protected override bool HasMemberNotNullAttribute(MemberInfo mi) => mi.Name.Contains("NotNull");
    }
}
