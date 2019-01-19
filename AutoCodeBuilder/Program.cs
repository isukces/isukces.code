using isukces.code;
using isukces.code.Ammy;
using isukces.code.AutoCode;

namespace AutoCodeBuilder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var myAssembly  = typeof(Program).Assembly;
            var solutionDir = CodeUtils.SearchFoldersUntilFileExists(myAssembly, "isukces.code.sln");
            if (solutionDir == null)
                return;
            var autoCodeGenerator = new AutoCodeGenerator
            {
                BaseDir = solutionDir
            };
            var ammyPropertyContainerMethodGenerator = new AmmyPropertyContainerMethodGenerator()
                .WithSkip<AmmyContainerBase>()
                .WithSkip<AmmyMixin>();
            autoCodeGenerator.CodeGenerators.Add(ammyPropertyContainerMethodGenerator);

            autoCodeGenerator.CodeGenerators.Add(new AmmyBindConverterHostGenerator());
            autoCodeGenerator.CodeGenerators.Add(new AmmyBindSourceHostGenerator());
            autoCodeGenerator.CodeGenerators.Add(new FluentBindGenerator());
            
            
            var saved        = false;
            var scanAssembly = typeof(CsLangInfo).Assembly;
            autoCodeGenerator.Make(scanAssembly, "isukces.code\\+AutoCode.cs", ref saved);
        }
    }
}