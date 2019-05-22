using System.Reflection;
using isukces.code;
using isukces.code.AutoCode;
using isukces.code.interfaces;
using JetBrains.Annotations;

namespace EqualityGeneratorSample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
#if DEBUG
            WriteAutoCode();
#endif
        }

        private static void WriteAutoCode()
        {
            IMemberNullValueChecker nullChecker = new MyNullChecker();
            var dirProvider = SlnAssemblyBaseDirectoryProvider.Make<Program>("EqualityGenerator.sln");
            IAssemblyFilenameProvider provider = new SimpleAssemblyFilenameProvider(dirProvider, "+AutoCode.cs");

            var acg = new AutoCodeGenerator(provider)
                .WithGenerator(new Generators.EqualityGenerator(nullChecker));
            acg.BeforeSave += (a, eventArgs) => { eventArgs.File.BeginContent = "// ReSharper disable All"; };
            acg.Make<Program>();
            if (acg.AnyFileSaved)
                throw new RecompileException();
        }
    }

    internal class MyNullChecker : AbstractMemberNullValueChecker
    {
        protected override bool HasMemberNotNullAttribute(MemberInfo mi)
        {
            // uses Jetbrain annotations
            // remember about JETBRAINS_ANNOTATIONS compiler directive
            var attr = mi.GetCustomAttribute<NotNullAttribute>();
            return attr != null;
        }
    }
}