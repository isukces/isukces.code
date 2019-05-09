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
            var solutionDir =
                typeof(Program).FindFileHereOrInParentDirectories("EqualityGenerator.sln");
            IMemberNullValueChecker nullChecker = new MyNullChecker();
            var acg = new AutoCodeGenerator
            {
                BaseDir = solutionDir
            }.WithGenerator(new Generators.EqualityGenerator(nullChecker));
            acg.BeforeSave += (a, eventArgs) => { eventArgs.File.BeginContent = "// ReSharper disable All"; };

            var anyFileSaved = false;
            acg.Make(typeof(Program).Assembly, "EqualityGeneratorSample\\+AutoCode.cs", ref anyFileSaved);
            
            if (anyFileSaved)
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