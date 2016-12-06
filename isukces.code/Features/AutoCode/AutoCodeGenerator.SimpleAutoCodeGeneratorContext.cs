using System;
using isukces.code.interfaces;

namespace isukces.code.AutoCode
{
    public partial class AutoCodeGenerator
    {
        private class SimpleAutoCodeGeneratorContext : IAutoCodeGeneratorContext
        {
            #region Constructors

            public SimpleAutoCodeGeneratorContext(Func<Type, CsClass> getOrCreateClassFunc,
                Action<string> addNamespaceAction, Func<Type, object> resolveConfigFunc)
            {
                GetOrCreateClassFunc = getOrCreateClassFunc;
                AddNamespaceAction = addNamespaceAction;
                ResolveConfigFunc = resolveConfigFunc;
            }

            #endregion

            #region Instance Methods

            public void AddNamespace(string namepace)
            {
                AddNamespaceAction(namepace);
            }

            public CsClass GetOrCreateClass(Type type)
            {
                return GetOrCreateClassFunc(type);
            }

            public T ResolveConfig<T>() where T : class, IAutoCodeConfiguration, new()
            {
                return (T)ResolveConfigFunc(typeof(T));
            }

            #endregion

            #region Properties

            public Func<Type, object> ResolveConfigFunc { get; set; }
            public Func<Type, CsClass> GetOrCreateClassFunc { get; set; }
            public Action<string> AddNamespaceAction { get; set; }

            #endregion
        }
    }
}