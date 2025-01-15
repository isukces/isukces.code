using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using iSukces.Code.Interfaces;

namespace iSukces.Code;

public static class IsukcesCodeExtensions
{
    public static DirectoryInfo? ScanDirectoryUp(DirectoryInfo? dir, Predicate<DirectoryInfo> accept)
    {
        while (dir is not null)
        {
            if (accept(dir))
                return dir;
            dir = dir.Parent;
        }

        return null;
    }

    public static DirectoryInfo? FindProjectRootPath(DirectoryInfo? dir)
    {
        return ScanDirectoryUp(dir, a => a.GetFiles("*.csproj").Length != 0);
    }

    public static DirectoryInfo? FindFileHereOrInParentDirectories(this DirectoryInfo di, string fileName)
    {
        return ScanDirectoryUp(di, x =>
        {
            var fi = Path.Combine(x.FullName, fileName);
            return File.Exists(fi);
        });
    }

    public static DirectoryInfo? FindFileHereOrInParentDirectories(this Type type, string fileName)
    {
        return type
#if COREFX
                .GetTypeInfo()
#endif
            .Assembly.FindFileHereOrInParentDirectories(fileName);
    }

    public static DirectoryInfo? FindFileHereOrInParentDirectories(this Assembly a, string fileName)
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

    public static void WriteAttributes(this ICsCodeWriter writer, ICollection<ICsAttribute>? attributes)
    {
        if (attributes is null || attributes.Count == 0)
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
        if (res is not INamespaceContainer container)
            return (CsType)typeName.FullName;
        var info = container.GetNamespaceInfo(typeName.Namespace);
        
        if (info.TryGetTypeName(typeName.Name, out var type))
            return type;
        return (CsType)typeName.FullName;
    }
}
