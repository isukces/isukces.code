using isukces.code;
using isukces.code.Ammy;
using isukces.code.AutoCode;

namespace AutoCodeBuilder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            EmitTypeAttribute.IgnoreEmitTypeAttribute = true; 
            
            var myAssembly  = typeof(Program).Assembly;
            var solutionDir = CodeUtils.SearchFoldersUntilFileExists(myAssembly, "isukces.code.sln");
            if (solutionDir == null)
                return;
            var dirProvider = SlnAssemblyBaseDirectoryProvider.Make<Program>("isukces.code.sln"); 
            IAssemblyFilenameProvider provider= new SimpleAssemblyFilenameProvider(dirProvider, "+AutoCode.cs");
            var autoCodeGenerator = new AutoCodeGenerator(provider);
            autoCodeGenerator.FileNamespaces.Add("isukces.code");
            var ammyPropertyContainerMethodGenerator = new AmmyPropertyContainerMethodGenerator()
                .WithSkip<AmmyContainerBase>()
                .WithSkip<AmmyMixin>();
            autoCodeGenerator.CodeGenerators.Add(ammyPropertyContainerMethodGenerator);

            // autoCodeGenerator.CodeGenerators.Add(new AmmyBindConverterHostGenerator());
            // autoCodeGenerator.CodeGenerators.Add(new AmmyBindSourceHostGenerator());
            autoCodeGenerator.CodeGenerators.Add(new FluentBindGenerator());
            
            var scanAssembly = typeof(CsLangInfo).Assembly;
            autoCodeGenerator.Make(scanAssembly);
        }
    }
}