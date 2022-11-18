using iSukces.Code.AutoCode;

namespace AmmySamples
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IAssemblyFilenameProvider pro =
                new SimpleAssemblyFilenameProvider(new AssemblyBaseDirectoryProvider(), "++AutoCode.cs");

            var acg = new AutoCodeGenerator(pro);
            acg.CodeGenerators.Clear();
            acg.Make<Program>();
        }
    }

    public sealed class Sample
    {
        public string Name { get; set; }
    }
}
