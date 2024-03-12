#nullable enable
using System;
using System.Reflection;

namespace iSukces.Code.AutoCode;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
public sealed class AssumeDefinedNamespaceAttribute : Attribute
{
    public static FileScopeNamespaceConfiguration? Get(Type type)
    {
        var att = type.GetCustomAttribute<AssumeDefinedNamespaceAttribute>(false);
        if (att is not null)
            return FileScopeNamespaceConfiguration.AssumeDefined(type.Namespace);
        return null;
    }
}
