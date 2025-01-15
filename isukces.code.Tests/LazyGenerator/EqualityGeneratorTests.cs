using System.Reflection;
using System.Runtime.CompilerServices;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;
using Xunit;

namespace iSukces.Code.Tests.LazyGenerator;

public partial class LazyGeneratorTests
{
    private static void CompareCode(bool addFrameworkPrefix, string code,
        [CallerMemberName] string? method = null, [CallerFilePath] string? file = null)
    {
        var resCs = "_res.cs";
        if (addFrameworkPrefix)
        {
#if NET9_0_OR_GREATER
            resCs = "_res.net9.cs1";
#else
            resCs = "_res.net8.cs1";
#endif
        }
        TestUtils.CompareWithResource(code, "iSukces.Code.Tests.LazyGenerator.", method, file, resCs);
    }

    private static void DoTest<T>(
        bool addFrameworkPrefix = false,
        [CallerMemberName] string? method = null, [CallerFilePath] string? file = null)
    {
        IMemberNullValueChecker c   = new MyValueChecker();
        var                     q   = new Generators.LazyGenerator();
        var            ctx = new TestContext();
        q.Generate(typeof(T), ctx);
        CompareCode(addFrameworkPrefix, ctx.Code, method, file);
    }


    [Fact]
    public void T01_Should_create()
    {
        DoTest<SampleClass>(true);
    }


    [Auto.EqualityGeneratorAttribute]
    public struct SampleStruct
    {
        public int IntValue { get; set; }
    }

    [Auto.EqualityGeneratorAttribute]
    public partial class SampleClass
    {
        public int IntValue { get; set; }

        [Auto.LazyAttribute(SyncObjectName = "_sync")]
        // ReSharper disable once MemberCanBeMadeStatic.Local
        private int LazyValue()
        {
            return 1;
        } 
            
        [Auto.LazyAttribute(SyncObjectName = "_sync", Name = "OtherName", FieldName = "_xotherNameField")]
        // ReSharper disable once MemberCanBeMadeStatic.Local
        private string LazyText()
        {
            return "";
        } 
            
            
        [Auto.LazyAttribute(UseLazyObject = true)]
        // ReSharper disable once MemberCanBeMadeStatic.Local
        private string LazyOtherTextAsMethod()
        {
            return "";
        } 

        [Auto.LazyAttribute(UseLazyObject = true, Target = Auto.LazyMemberType.Property)]
        // ReSharper disable once MemberCanBeMadeStatic.Local
        private string LazyOtherTextAsProperty()
        {
            return "";
        } 
            
    }


    public class MyValueChecker : AbstractMemberNullValueChecker
    {
        protected override bool HasMemberNotNullAttribute(MemberInfo mi) => mi.Name.Contains("NotNull");
    }
}
