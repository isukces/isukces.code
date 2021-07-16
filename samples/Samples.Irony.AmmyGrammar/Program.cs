using System;
using System.Collections.Generic;
using Irony.Parsing;
using iSukces.Code;
using iSukces.Code.AutoCode;
using iSukces.Code.CodeWrite;
using iSukces.Code.Irony;
using Samples.Irony.AmmyGrammar.Autocode;

namespace Samples.Irony.AmmyGrammar
{
    internal class Program
    {
        public static void GenerateAll()
        {
            var dirProvider = SlnAssemblyBaseDirectoryProvider.Make<Program>("isukces.code.sln", "samples");
            var file        = new CsFile();
            IronyGrammarConfigurator.AddImportNamespaces(file);
            var classes = new Dictionary<TypeProvider, CsClass>();

            IAutoCodeGeneratorContext ctx =
                new AutoCodeGenerator.SimpleAutoCodeGeneratorContext(file,
                    type =>
                    {
                        var c = file.GetOrCreateClass(type, classes);
                        c.IsPartial = true;
                        return c;
                    });
            var cfg = new AmmyGrammarConfigurator();
            var gen = cfg.Build();
            gen.Generate(ctx);
            var filename = dirProvider.GetFileName(cfg.GetType().Assembly, "AmmyGrammar+Auto.cs");
            file.SaveIfDifferent(filename);
            Console.WriteLine("Ammy grammar created!");
        }

        private static void Main(string[] args)
        {
            GenerateAll();
            TestGrammar();
        }

        private static void TestGrammar()
        {
            var grammar  = new AmmyGrammar();
            var language = new LanguageData(grammar);

            void Show(string s)
            {
                Console.WriteLine(" === " + s + " ===");
                foreach (var q in IronyDebugger.GetDebugLines(s, language))
                    Console.WriteLine(q);
            }
            foreach (var i in language.Errors)
            {
                Show(i.State.Name);
                Console.WriteLine("ERROR: " + i.Message);
                
                Show("S44");
                Show("S19");
                
                
                return;
            }
        }
    }
}