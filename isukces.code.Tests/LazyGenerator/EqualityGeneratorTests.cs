using System.Reflection;
using System.Runtime.CompilerServices;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;
using Xunit;

namespace iSukces.Code.Tests.LazyGenerator
{
    public partial class LazyGeneratorTests
    {
        private static void CompareCode(string code,
            [CallerMemberName] string? method = null, [CallerFilePath] string? file = null)
        {
            TestUtils.CompareWithResource(code, "iSukces.Code.Tests.LazyGenerator.", method, file, "_res.cs");
        }

        private static void DoTest<T>(
            [CallerMemberName] string? method = null, [CallerFilePath] string? file = null)
        {
            IMemberNullValueChecker c   = new MyValueChecker();
            var                     q   = new Generators.LazyGenerator();
            var                     ctx = new TestContext();
            q.Generate(typeof(T), ctx);
            CompareCode(ctx.Code, method, file);
        }


        [Fact]
        public void T01_Should_create()
        {
            DoTest<SampleClass>();
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
}