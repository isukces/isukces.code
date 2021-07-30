using System;
using iSukces.Code.CodeWrite;

namespace iSukces.Code.AutoCode
{
    public partial class AutoCodeGenerator
    {
        private sealed class ContextWrapper 
        {

            public ContextWrapper(Func<CsFile, IAutoCodeGeneratorContext> contextFactory, CsOutputFileInfo sourceInfo)
            {
                SourceInfo = sourceInfo;
                File          = new CsFile();
                Context  = contextFactory(File);
            }

            public IAutoCodeGeneratorContext Context    { get; }
            
            public CsOutputFileInfo SourceInfo { get; }
            
            public CsClass GetOrCreateClass(TypeProvider type)
            {
                // have to be protected due to CreateAutoCodeGeneratorContext method 
                return File.GetOrCreateClass(type);
            }
         
            public CsFile File { get; }
        }
    }
}
