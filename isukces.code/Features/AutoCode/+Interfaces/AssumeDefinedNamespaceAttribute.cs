#nullable enable
using System;
using System.Reflection;

namespace iSukces.Code.AutoCode;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
public sealed class AssumeDefinedNamespaceAttribute : Attribute
{
    public static FileScopeNamespaceConfiguration? Get(Type type)
    {
        var att                = type.GetCustomAttribute<AssumeDefinedNamespaceAttribute>(false);
        var typeNamespace = type.Namespace;
        return att is null || string.IsNullOrEmpty(typeNamespace)
            ? null
            : FileScopeNamespaceConfiguration.AssumeDefined(typeNamespace);
    }
}
