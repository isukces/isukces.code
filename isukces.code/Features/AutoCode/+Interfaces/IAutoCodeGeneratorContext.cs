using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using iSukces.Code.Interfaces;

namespace iSukces.Code.AutoCode;

public interface IAutoCodeGeneratorContext:IFileSavedNotifier
{
    void AddNamespace(string namepace);

    CsClass GetOrCreateClass(TypeProvider type);

    CsNamespace GetOrCreateNamespace(string namespaceName);

    IList<object> Tags { get; }

    bool AnyFileSaved { get; }
        
    ITypeNameResolver FileLevelResolver { get; }
}

public interface IFinalizableAutoCodeGeneratorContext : IAutoCodeGeneratorContext
{
    void FinalizeContext(Assembly assembly);
}

public static class AutoCodeGeneratorContextExtensions
{
    extension(IAutoCodeGeneratorContext self)
    {
        public CsClass GetOrCreateClass(CsType typeName, CsNamespaceMemberKind kind)
        {
            return self.GetOrCreateClass(TypeProvider.FromTypeName(typeName, kind));
        }

        public void AddNamespace(Type type)
        {
            var ns = type.Namespace;
            if (!string.IsNullOrEmpty(ns))
                self.AddNamespace(ns);
        }

        public void AddNamespace<T>()
        {
            AddNamespace(self, typeof(T));
        }

        public CsClass GetOrCreateClass(Type type)
        {
            if (self is null) throw new ArgumentNullException(nameof(self));
            if (type is null)
                throw new NullReferenceException(nameof(type));
            return self.GetOrCreateClass(TypeProvider.FromType(type));
        }
    }
}
