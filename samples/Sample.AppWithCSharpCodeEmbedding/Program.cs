using System.Reflection;
using iSukces.Code.AutoCode;
using Sample.AppWithCSharpCodeEmbedding.AutoCode;

namespace Sample.AppWithCSharpCodeEmbedding
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateAutoCode();
        }

        private static void CreateAutoCode()
        {
            var prov2 = SlnAssemblyBaseDirectoryProvider.Make<DemoAutoCodeGenerator>("isukces.code.sln", "samples");
            IAssemblyFilenameProvider provider = new SimpleAssemblyFilenameProvider(prov2, "--Autocode--.cs");
            var                       gen         = new DemoAutoCodeGenerator(provider);
            gen.WithGenerator(new Generators.EqualityGenerator(new JetbrainsAttributeNullValueChecker()));
            gen.TypeBasedOutputProvider = new CodeFilePathContextProvider();
            gen.Make<DemoAutoCodeGenerator>();
        }
    }
}
