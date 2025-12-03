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
    ///     Namespace is not empty and was found in usings
    /// </summary>
    Found,

    /// <summary>
    ///     Namespace is not empty and was not found in usings
    /// </summary>
    NotFound,

    /// <summary>
    ///     Namespace is empty
    /// </summary>
    Empty
}

public readonly record struct UsingInfo(NamespaceSearchResult SearchResult, string? Alias = null)
{
    public CsType AddAlias(string shortTypeName)
    {
        return new CsType(Alias, shortTypeName);
    }

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

        var addAlias = SearchResult == NamespaceSearchResult.Found;
        type = addAlias
            ? AddAlias(shortTypeName)
            : default;
        return addAlias;
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
