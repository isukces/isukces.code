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

    public UsingInfo GetNamespaceInfo(string namespaceName)
    {
        return new UsingInfo(false);
    }

    public static FullNameTypeNameResolver Instance => InstanceHolder.SingleInstance;

    private sealed class InstanceHolder
    {
        public static readonly FullNameTypeNameResolver SingleInstance = new();
    }
}