#nullable disable
using iSukces.Code.AutoCode;

namespace Sample.AppWithCSharpCodeEmbedding.AutoCode
{
    public class DemoAutoCodeGenerator : AutoCodeGenerator
    {
        public DemoAutoCodeGenerator(IAssemblyFilenameProvider filenameProvider)
            : base(filenameProvider)
        {
        }
    }
}

