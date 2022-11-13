using System;
using System.Reflection;

namespace iSukces.Code.AutoCode
{
    public partial class AutoCodeGenerator
    {
        private sealed class ContextWrapper 
        {

            public ContextWrapper(Func<CsFile, Assembly, IAutoCodeGeneratorContext> contextFactory, 
                CsOutputFileInfo sourceInfo,
                Assembly assembly)
            {
                SourceInfo = sourceInfo;
                File          = new CsFile();
                Context  = contextFactory(File, assembly);
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
