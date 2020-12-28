using System;
using System.Collections.Generic;
using System.IO;
using iSukces.Code.CodeWrite;
using iSukces.Code.Interfaces;

namespace iSukces.Code.AutoCode
{
    public partial class AutoCodeGenerator
    {
        public class SimpleAutoCodeGeneratorContext : IFinalizableAutoCodeGeneratorContext
        {
            public SimpleAutoCodeGeneratorContext(Func<TypeProvider, CsClass> getOrCreateClassFunc,
                Action<string> addNamespaceAction, Func<string, CsNamespace> getOrCreateNamespaceFunc)
            {
                GetOrCreateClassFunc     = getOrCreateClassFunc;
                AddNamespaceAction       = addNamespaceAction;
                GetOrCreateNamespaceFunc = getOrCreateNamespaceFunc;
            }

            public SimpleAutoCodeGeneratorContext(CsFile file, Func<TypeProvider, CsClass> getOrCreateClassFunc)
            {
                GetOrCreateClassFunc     = getOrCreateClassFunc;
                AddNamespaceAction       = file.AddImportNamespace;
                GetOrCreateNamespaceFunc = file.GetOrCreateNamespace;
                FileLevelResolver        = file;
            }

            public void AddNamespace(string namepace)
            {
                AddNamespaceAction(namepace);
            }

            public void FileSaved(FileInfo fileInfo)
            {
                AnyFileSaved = true;
            }

            public virtual void Finalize()
            {
            }

            public CsClass GetOrCreateClass(TypeProvider type) => GetOrCreateClassFunc(type);

            public CsNamespace GetOrCreateNamespace(string namespaceName) => GetOrCreateNamespaceFunc(namespaceName);

            public IList<object> Tags { get; } = new List<object>();


            public Func<string, CsNamespace>   GetOrCreateNamespaceFunc { get; }
            public Func<TypeProvider, CsClass> GetOrCreateClassFunc     { get; }
            public Action<string>              AddNamespaceAction       { get; }
            public bool                        AnyFileSaved             { get; set; }

            public ITypeNameResolver FileLevelResolver { get; set; }
        }
    }
}