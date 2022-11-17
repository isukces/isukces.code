namespace AutoCodeBuilder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
#if OLD
            Make1();
            Make2();
#endif
        }
#if OLD
        private static void Make2()
        {
#if DEBUG

            EmitTypeAttribute.IgnoreEmitTypeAttribute = true;

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
#if AMMY
            EmitTypeAttribute.IgnoreEmitTypeAttribute = true;
#endif

            var myAssembly = typeof(Program).Assembly;
            var solutionDir = CodeUtils.SearchFoldersUntilFileExists(myAssembly, "iSukces.Code.sln");
            if (solutionDir == null)
                return;
            var                       dirProvider = SlnAssemblyBaseDirectoryProvider.Make<Program>("iSukces.Code.sln");
            IAssemblyFilenameProvider provider = new SimpleAssemblyFilenameProvider(dirProvider, "+AutoCode.cs");
            var                       autoCodeGenerator = new AutoCodeGenerator(provider);
            autoCodeGenerator.FileNamespaces.Add("iSukces.Code");
#if AMMY
            var ammyPropertyContainerMethodGenerator = new AmmyPropertyContainerMethodGenerator()
                .WithSkip<AmmyContainerBase>()
                .WithSkip<AmmyMixin>();
            autoCodeGenerator.CodeGenerators.Add(ammyPropertyContainerMethodGenerator);

            // autoCodeGenerator.CodeGenerators.Add(new AmmyBindConverterHostGenerator());
            // autoCodeGenerator.CodeGenerators.Add(new AmmyBindSourceHostGenerator());
            // autoCodeGenerator.CodeGenerators.Add(new FluentBindGenerator());
            // autoCodeGenerator.CodeGenerators.Add(new SystemColorsGenerator());
#endif

            var scanAssembly = typeof(CsLangInfo).Assembly;
            autoCodeGenerator.Make(scanAssembly);
        }
#endif
    }
}
