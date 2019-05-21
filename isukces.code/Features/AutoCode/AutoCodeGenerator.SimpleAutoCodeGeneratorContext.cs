using System;
using System.Collections.Generic;
using System.IO;
using isukces.code.interfaces;

namespace isukces.code.AutoCode
{
    public partial class AutoCodeGenerator
    {
        private class SimpleAutoCodeGeneratorContext : IAutoCodeGeneratorContext
        {
            public SimpleAutoCodeGeneratorContext(Func<TypeProvider, CsClass> getOrCreateClassFunc,
                Action<string> addNamespaceAction, Func<Type, object> resolveConfigFunc)
            {
                GetOrCreateClassFunc = getOrCreateClassFunc;
                AddNamespaceAction   = addNamespaceAction;
                ResolveConfigFunc    = resolveConfigFunc;
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

            public T ResolveConfig<T>() where T : class, IAutoCodeConfiguration, new()
            {
                return (T)ResolveConfigFunc(typeof(T));
            }

            public IList<object> Tags { get; } = new List<object>();

            public Func<Type, object>          ResolveConfigFunc    { get; }
            public Func<TypeProvider, CsClass> GetOrCreateClassFunc { get; }
            public Action<string>              AddNamespaceAction   { get; }
            public bool                        AnyFileSaved         { get; set; }
        }
    }
}