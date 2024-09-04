#nullable enable
using System;
using System.Collections.Generic;

namespace iSukces.Code.Interfaces;

public interface INamespaceContainer
{
    UsingInfo GetNamespaceInfo(string? namespaceName);
}

public readonly record struct UsingInfo(bool IsKnown, string? Alias = null)
{
    public bool IsKnownWithoutAlias()
    {
        return IsKnown && string.IsNullOrEmpty(Alias);
    }

    public bool TryGetTypeName(string shortTypeName, out CsType type)
    {
        type = IsKnown
            ? AddAlias(shortTypeName)
            : default;
        return IsKnown;
    }

    public CsType AddAlias(string shortTypeName)
    {
        var n = string.IsNullOrEmpty(Alias)
            ? shortTypeName
            : $"{Alias}.{shortTypeName}";
        return new CsType(n);
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
        if (namespaces == null || namespaces.Length == 0)
            return;
        for (var index = 0; index < namespaces.Length; index++)
        {
            var ns = namespaces[index];
            self.AddImportNamespace(ns);
        }
    }

    public static void AddImportNamespace(this INamespaceCollection self, params Type[]? types)
    {
        if (types == null || types.Length == 0)
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
        return info.IsKnown
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