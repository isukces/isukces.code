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
    public static CsClass GetOrCreateClass(this IAutoCodeGeneratorContext? self, Type type)
    {
        if (self is null) throw new ArgumentNullException(nameof(self));
        if (type is null)
            throw new NullReferenceException(nameof(type));
        return self.GetOrCreateClass(TypeProvider.FromType(type));
    }
    
    public static CsClass GetOrCreateClass(this IAutoCodeGeneratorContext self, CsType typeName, CsNamespaceMemberKind kind)
    {
        return self.GetOrCreateClass(TypeProvider.FromTypeName(typeName, kind));
    }
}
    
public static class AutoCodeGeneratorContextExt
{
    public static void AddNamespace(this IAutoCodeGeneratorContext src, Type type)
    {
        var ns = type.Namespace;
        if (!string.IsNullOrEmpty(ns))
            src.AddNamespace(ns);
    }

    public static void AddNamespace<T>(this IAutoCodeGeneratorContext src)
    {
        AddNamespace(src, typeof(T));
    }
}
