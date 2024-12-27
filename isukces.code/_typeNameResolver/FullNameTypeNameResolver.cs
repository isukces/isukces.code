#nullable enable
using System;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;

namespace iSukces.Code;

public sealed class FullNameTypeNameResolver : ITypeNameResolver, INamespaceContainer
{
    private FullNameTypeNameResolver()
    {
    }

    public CsType GetTypeName(Type type)
    {
        return GeneratorsHelper.GetTypeName(this, type);
    }

    public UsingInfo GetNamespaceInfo(string? namespaceName)
    {
        var isKnown = string.IsNullOrEmpty(namespaceName)
            ? NamespaceSearchResult.Empty
            : NamespaceSearchResult.NotFound;
        return new UsingInfo(isKnown);
    }

    public string? TryGetTypeAlias(TypeProvider type) => null;

    public static FullNameTypeNameResolver Instance => InstanceHolder.SingleInstance;

    private sealed class InstanceHolder
    {
        public static readonly FullNameTypeNameResolver SingleInstance = new();
    }
}