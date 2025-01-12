#nullable disable
using iSukces.Code;
using iSukces.Code.AutoCode;

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
#if DEBUGx
            
            var myAssembly = typeof(Program).Assembly;
            var solutionDir = CodeUtils.SearchFoldersUntilFileExists(myAssembly, "iSukces.Code.sln");
            if (solutionDir == null)
                return;
            var                       dirProvider = SlnAssemblyBaseDirectoryProvider.Make<Program>("iSukces.Code.sln");
            IAssemblyFilenameProvider provider = new SimpleAssemblyFilenameProvider(dirProvider, "+AutoCode.cs");
            var                       autoCodeGenerator = new AutoCodeGenerator(provider);
            
            
            // autoCodeGenerator.FileNamespaces.Add("iSukces.Code");
            autoCodeGenerator.FileNamespaces.Add("Irony.Interpreter.Ast");
            autoCodeGenerator.FileNamespaces.Add("Irony.Parsing");
            autoCodeGenerator.CodeGenerators.Add(new IronParserGenerator(AmmyGrammarAutogeneratorInfo.Items(), typeof(AmmyGrammar)));
            var scanAssembly = typeof(AmmyGrammarAutogeneratorInfo).Assembly;
            autoCodeGenerator.Make(scanAssembly);

#endif
        }

        private static void Make1()
        {
            var myAssembly = typeof(Program).Assembly;
            var solutionDir = CodeUtils.SearchFoldersUntilFileExists(myAssembly, "iSukces.Code.sln");
            if (solutionDir == null)
                return;
            var                       dirProvider = SlnAssemblyBaseDirectoryProvider.Make<Program>("iSukces.Code.sln");
            IAssemblyFilenameProvider provider = new SimpleAssemblyFilenameProvider(dirProvider, "+AutoCode.cs");
            var                       autoCodeGenerator = new AutoCodeGenerator(provider);
            autoCodeGenerator.FileNamespaces.Add("iSukces.Code");


            var scanAssembly = typeof(CsLangInfo).Assembly;
            autoCodeGenerator.Make(scanAssembly);
        }
    }
}

