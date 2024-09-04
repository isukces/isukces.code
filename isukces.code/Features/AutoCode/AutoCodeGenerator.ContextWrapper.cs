#nullable enable
using System;
using System.Reflection;

namespace iSukces.Code.AutoCode;

public delegate IAutoCodeGeneratorContext ContextFactoryDelegate
    (CsFile file, Assembly assembly);

public partial class AutoCodeGenerator
{
    private sealed class ContextWrapper 
    {
        public ContextWrapper(ContextFactoryDelegate contextFactory, CsOutputFileInfo sourceInfo, Assembly assembly)
        {
            SourceInfo = sourceInfo;
            File       = CsFileFactory.Instance.Create(assembly, typeof(ContextWrapper), sourceInfo);
            Context    = contextFactory(File, assembly);
        }

        public IAutoCodeGeneratorContext Context { get; }
            
        public CsOutputFileInfo SourceInfo { get; }
            
        public CsClass GetOrCreateClass(TypeProvider type)
        {
            // have to be protected due to CreateAutoCodeGeneratorContext method 
            return File.GetOrCreateClass(type);
        }
         
        public CsFile File { get; }
    }
}