using System;
using System.Collections.Generic;
using System.IO;
using isukces.code.CodeWrite;
using isukces.code.interfaces;

namespace isukces.code.AutoCode
{
    public partial class AutoCodeGenerator
    {
        private class SimpleAutoCodeGeneratorContext : IAutoCodeGeneratorContext
        {
            public SimpleAutoCodeGeneratorContext(Func<TypeProvider, CsClass> getOrCreateClassFunc,
                Action<string> addNamespaceAction, Func<string, CsNamespace> getOrCreateNamespaceFunc)
            {
                GetOrCreateClassFunc = getOrCreateClassFunc;
                AddNamespaceAction   = addNamespaceAction;
                GetOrCreateNamespaceFunc = getOrCreateNamespaceFunc;
            }
            public SimpleAutoCodeGeneratorContext(CsFile file, Func<TypeProvider, CsClass> getOrCreateClassFunc)
            {
                GetOrCreateClassFunc     = getOrCreateClassFunc;
                AddNamespaceAction = file.AddImportNamespace;
                GetOrCreateNamespaceFunc = file.GetOrCreateNamespace;

            }

            public void AddNamespace(string namepace)
            {
                AddNamespaceAction(namepace);
            }

            public void FileSaved(FileInfo fileInfo)
            {
                AnyFileSaved = true;
            }

            public CsClass GetOrCreateClass(TypeProvider type)
            {
                return GetOrCreateClassFunc(type);
            }

            public CsNamespace GetOrCreateNamespace(string namespaceName)
            {
                return GetOrCreateNamespaceFunc(namespaceName);
            }

            public IList<object> Tags { get; } = new List<object>();


            public Func<string, CsNamespace>   GetOrCreateNamespaceFunc { get; }
            public Func<TypeProvider, CsClass> GetOrCreateClassFunc     { get; }
            public Action<string>              AddNamespaceAction       { get; }
            public bool                        AnyFileSaved             { get; set; }
        }
    }
}