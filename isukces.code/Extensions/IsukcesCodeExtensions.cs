using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using iSukces.Code.Interfaces;
using JetBrains.Annotations;

namespace iSukces.Code;

public static class IsukcesCodeExtensions
{
    public static DirectoryInfo ScanDirectoryUp([CanBeNull] DirectoryInfo dir, Predicate<DirectoryInfo> accept)
    {
        while (dir != null)
        {
            if (accept(dir))
                return dir;
            dir = dir.Parent;
        }

        return null;
    }

    [CanBeNull]
    public static DirectoryInfo FindProjectRootPath([CanBeNull] DirectoryInfo dir)
    {
        return ScanDirectoryUp(dir, a => a.GetFiles("*.csproj").Length != 0);
    }

    [CanBeNull]
    public static DirectoryInfo FindFileHereOrInParentDirectories(this DirectoryInfo di, string fileName)
    {
        return ScanDirectoryUp(di, x =>
        {
            var fi = Path.Combine(x.FullName, fileName);
            return File.Exists(fi);
        });
    }

    public static DirectoryInfo FindFileHereOrInParentDirectories(this Type type, string fileName)
    {
        return type
#if COREFX
                .GetTypeInfo()
#endif
            .Assembly.FindFileHereOrInParentDirectories(fileName);
    }

    public static DirectoryInfo FindFileHereOrInParentDirectories(this Assembly a, string fileName)
    {
        var di = new FileInfo(a.Location).Directory;
        di = di.FindFileHereOrInParentDirectories(fileName);
        return di;
    }

    public static CsMethod WithAggressiveInlining(this CsMethod method, ITypeNameResolver resolver, bool add = true)
    {
        if (!add)
            return method;
        var att = AggressiveInlining(resolver);
        return method.WithAttribute(att);
    }

    private static CsAttribute AggressiveInlining(ITypeNameResolver resolver)
    {
        var arg = resolver.GetTypeName<MethodImplOptions>()
            .GetMemberCode(nameof(MethodImplOptions.AggressiveInlining));
        var att = CsAttribute.Make<MethodImplAttribute>(resolver)
            .WithArgumentCode(arg);
        return att;
    }

    public static void WriteAttributes(this ICsCodeWriter writer, ICollection<ICsAttribute> attributes)
    {
        if (attributes == null || attributes.Count == 0)
            return;
        foreach (var j in attributes)
        {
            var close = writer.OpenCompilerIf(j);
            writer.WriteLine("[{0}]", j.Code);
            writer.CloseCompilerIf(close);
        }
    }

    public static CsType GetTypeName(this ITypeNameResolver res, NamespaceAndName typeName)
    {
        if (res is INamespaceContainer container)
            if (container.IsKnownNamespace(typeName.Namespace))
                return (CsType)typeName.Name;
        return (CsType)typeName.FullName;
    }
}