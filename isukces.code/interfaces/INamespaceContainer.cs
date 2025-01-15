using System;
using System.Collections.Generic;
using iSukces.Code.AutoCode;

namespace iSukces.Code.Interfaces;

public interface INamespaceContainer
{
    UsingInfo GetNamespaceInfo(string? namespaceName);

    string? TryGetTypeAlias(TypeProvider type);
}

public enum NamespaceSearchResult
{
    /// <summary>
    /// Namespace is not empty and was found in usings
    /// </summary>
    Found,
    
    /// <summary>
    /// Namespace is not empty and was not found in usings
    /// </summary>
    NotFound,
    
    /// <summary>
    /// Namespace is empty
    /// </summary>
    Empty
}

public readonly record struct UsingInfo(NamespaceSearchResult SearchResult, string? Alias = null)
{
    public bool IsKnownWithoutAlias()
    {
        if (SearchResult == NamespaceSearchResult.Empty)
            return true;
        return SearchResult == NamespaceSearchResult.Found && string.IsNullOrEmpty(Alias);
    }

    public bool TryGetTypeName(string shortTypeName, out CsType type)
    {
        if (SearchResult == NamespaceSearchResult.Empty)
        {
            type = new CsType(shortTypeName);
            return true;
        }
        var addAlias = SearchResult==NamespaceSearchResult.Found;
        type = addAlias
            ? AddAlias(shortTypeName)
            : default;
        return addAlias;
    }

    public CsType AddAlias(string shortTypeName)
    { 
        return new CsType(Alias, shortTypeName);
    }
}

public interface INamespaceCollection
{
    void AddImportNamespace(string? ns, string? alias = null);
}

public static class NamespaceCollectionExt
{
    public static void AddImportNamespace(this INamespaceCollection self, params string[]? namespaces)
    {
        if (namespaces is null || namespaces.Length == 0)
            return;
        for (var index = 0; index < namespaces.Length; index++)
        {
            var ns = namespaces[index];
            self.AddImportNamespace(ns);
        }
    }

    public static void AddImportNamespace(this INamespaceCollection self, params Type[]? types)
    {
        if (types is null || types.Length == 0)
            return;
        for (var index = 0; index < types.Length; index++)
        {
            var ns = types[index];
            self.AddImportNamespace(ns);
        }
    }

    public static void AddImportNamespace(this INamespaceCollection self, Type type, string? alias = null)
    {
        var ns = type.Namespace;
        if (!string.IsNullOrEmpty(ns))
            self.AddImportNamespace(type.Namespace, alias);
    }

    public static void AddImportNamespace<T>(this INamespaceCollection self, string? alias = null)
    {
        AddImportNamespace(self, typeof(T), alias);
    }

    [Obsolete("Use GetNamespaceInfo().IsKnownWithoutAlias()")]
    public static bool IsKnownNamespace(this INamespaceContainer self, string? namespaceName)
    {
        if (string.IsNullOrEmpty(namespaceName))
            return true;
        var a = self.GetNamespaceInfo(namespaceName);
        return a.IsKnownWithoutAlias();
    }

    public static CsType GetTypeName(this INamespaceContainer self, string namespaceName, string shortName)
    {
        var info = self.GetNamespaceInfo(namespaceName);
        return info.SearchResult != NamespaceSearchResult.NotFound
            ? info.AddAlias(shortName)
            : new CsType($"{namespaceName}.{shortName}");
    }
}

public interface IClassOwner : INamespaceContainer, ITypeNameResolver
{
}

public interface IEnumOwner : INamespaceContainer, ITypeNameResolver
{
    CsEnum AddEnum(CsEnum csEnum);

    /// <summary>
    ///     Enums
    /// </summary>
    IReadOnlyList<CsEnum> Enums { get; }
}

public interface INamespaceOwner : INamespaceContainer, ITypeNameResolver
{
}

public interface IConditional
{
    /// <summary>
    ///     Compiler directive required for element
    /// </summary>
    string? CompilerDirective { get; set; }
}

public static class ConditionalExtensions
{
    public static T WithCompilerDirective<T>(this T src, string directive)
        where T : IConditional
    {
        src.CompilerDirective = directive;
        return src;
    }
}
