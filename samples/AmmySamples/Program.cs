
using iSukces.Code.Ammy;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces.Ammy;

namespace AmmySamples
{
    internal class Program
    {
#if AMMY
        [AmmyBuilder]
        public static void AmmyInit(AmmyBuilderContext x)
        {
            x.EmbedInRelativeFile();
            x.AddImportNamespace("AmmySamples");

#if AMMY
            x.RegisterMixin<Sample>("SampleMixing")
                .WithProperty(a => a.Name, "Piotr");
#endif
        }
#endif

        private static void Main(string[] args)
        {
            IAssemblyFilenameProvider pro =
                new SimpleAssemblyFilenameProvider(new AssemblyBaseDirectoryProvider(), "++AutoCode.cs");

            var acg = new AutoCodeGenerator(pro);
            acg.CodeGenerators.Clear();
#if AMMY
            acg.WithGenerator(new AmmyAutocodeGenerator(pro));
#endif
            acg.Make<Program>();
        }
    }

    public sealed class Sample
    {
        public string Name { get; set; }
    }
}
