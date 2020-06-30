using Bitbrains.AmmyParser;
using isukces.code;
using isukces.code.Ammy;
using isukces.code.AutoCode;

namespace AutoCodeBuilder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Make1();
            Make2();
        }
        
        private static void Make2()
        {
            #if DEBUG

            EmitTypeAttribute.IgnoreEmitTypeAttribute = true;

            var myAssembly  = typeof(Program).Assembly;
            var solutionDir = CodeUtils.SearchFoldersUntilFileExists(myAssembly, "isukces.code.sln");
            if (solutionDir == null)
                return;
            var                       dirProvider       = SlnAssemblyBaseDirectoryProvider.Make<Program>("isukces.code.sln");
            IAssemblyFilenameProvider provider          = new SimpleAssemblyFilenameProvider(dirProvider, "+AutoCode.cs");
            var                       autoCodeGenerator = new AutoCodeGenerator(provider);
            
            
            // autoCodeGenerator.FileNamespaces.Add("isukces.code");
            autoCodeGenerator.FileNamespaces.Add("Irony.Interpreter.Ast");
            autoCodeGenerator.FileNamespaces.Add("Irony.Parsing");
            
            var ammyPropertyContainerMethodGenerator = new AmmyPropertyContainerMethodGenerator()
                .WithSkip<AmmyContainerBase>()
                .WithSkip<AmmyMixin>();
            autoCodeGenerator.CodeGenerators.Add(ammyPropertyContainerMethodGenerator);

            // autoCodeGenerator.CodeGenerators.Add(new AmmyBindConverterHostGenerator());
            // autoCodeGenerator.CodeGenerators.Add(new AmmyBindSourceHostGenerator());
            //autoCodeGenerator.CodeGenerators.Add(new FluentBindGenerator());
            // autoCodeGenerator.CodeGenerators.Add(new SystemColorsGenerator());
            autoCodeGenerator.CodeGenerators.Add(new IronParserGenerator(AmmyGrammarAutogeneratorInfo.Items(), typeof(AmmyGrammar)));

            var scanAssembly = typeof(AmmyGrammarAutogeneratorInfo).Assembly;
            autoCodeGenerator.Make(scanAssembly);

            #endif
        }

        private static void Make1()
        {
            EmitTypeAttribute.IgnoreEmitTypeAttribute = true;

            var myAssembly  = typeof(Program).Assembly;
            var solutionDir = CodeUtils.SearchFoldersUntilFileExists(myAssembly, "isukces.code.sln");
            if (solutionDir == null)
                return;
            var                       dirProvider       = SlnAssemblyBaseDirectoryProvider.Make<Program>("isukces.code.sln");
            IAssemblyFilenameProvider provider          = new SimpleAssemblyFilenameProvider(dirProvider, "+AutoCode.cs");
            var                       autoCodeGenerator = new AutoCodeGenerator(provider);
            autoCodeGenerator.FileNamespaces.Add("isukces.code");
            var ammyPropertyContainerMethodGenerator = new AmmyPropertyContainerMethodGenerator()
                .WithSkip<AmmyContainerBase>()
                .WithSkip<AmmyMixin>();
            autoCodeGenerator.CodeGenerators.Add(ammyPropertyContainerMethodGenerator);

            // autoCodeGenerator.CodeGenerators.Add(new AmmyBindConverterHostGenerator());
            // autoCodeGenerator.CodeGenerators.Add(new AmmyBindSourceHostGenerator());
            autoCodeGenerator.CodeGenerators.Add(new FluentBindGenerator());
            autoCodeGenerator.CodeGenerators.Add(new SystemColorsGenerator());

            var scanAssembly = typeof(CsLangInfo).Assembly;
            autoCodeGenerator.Make(scanAssembly);
        }
    }
}