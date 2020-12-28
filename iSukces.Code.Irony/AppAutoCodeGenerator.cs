#if DEBUGx
using System.Collections.Generic;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using iSukces.Code;
using iSukces.Code.AutoCode;
using iSukces.Code.CodeWrite;
using iSukces.Code.Interfaces;

namespace IronyParserTests.AutoCode
{
    public sealed class AppAutoCodeGenerator
    {
        public static void GenerateAll()
        {
            var app  = SlnAssemblyBaseDirectoryProvider.Make<AppAutoCodeGenerator>("IronyParserTests.sln");
            var file = new CsFile();
            file.AddImportNamespace(typeof(StatementListNode));
            file.AddImportNamespace(typeof(NonTerminal));

            var _classes = new Dictionary<TypeProvider, CsClass>();

            IAutoCodeGeneratorContext ctx =
                new AutoCodeGenerator.SimpleAutoCodeGeneratorContext(file,
                    type =>
                    {
                        var c = file.GetOrCreateClass(type, _classes);
                        c.IsPartial = true;
                        return c;
                    });
            GenerateIrony(ctx);
            file.SaveIfDifferent(app.GetFileName(typeof(AppAutoCodeGenerator).Assembly, "+AutoCode.cs"));
        }

        /*
        private static void GenerateIrony(IAutoCodeGeneratorContext ctx)
        {
            var cfg = new Ammy2AutocodeConfigurator();
            var gen = cfg.Build();
            gen.Generate(ctx);
        }*/
    }
}
#endif