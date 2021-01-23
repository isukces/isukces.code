using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using iSukces.Code.Interfaces;

namespace iSukces.Code
{
    public static class IsukcesCodeExtensions
    {
        public static DirectoryInfo FindFileHereOrInParentDirectories(this DirectoryInfo di, string fileName)
        {
            while (di != null)
            {
                if (!di.Exists)
                    return null;
                var fi = Path.Combine(di.FullName, fileName);
                if (File.Exists(fi))
                    return di;
                di = di.Parent;
            }

            return null;
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
            var att = CsAttribute.Make<MethodImplAttribute>(resolver)
                .WithArgumentCode(resolver.GetTypeName<MethodImplOptions>() + "." +
                                  nameof(MethodImplOptions.AggressiveInlining));
            return att;
        }
        
        public static string GetTypeName(this ITypeNameResolver res, NamespaceAndName typeName)
        {
            if (res is INamespaceContainer container)
                if (container.IsKnownNamespace(typeName.Namespace))
                    return typeName.Name;
            return typeName.FullName;
        }

        [Obsolete("use "+nameof(IsukcesCodeExtensions)+"."+nameof(GetTypeName))]
        public static string ReduceTypenameIfPossible(this CsClass csClass, string typeName)
        {
            if (typeName is null || csClass is null)
                return typeName;
            
            var referenceNamespace = csClass.GetNamespace();
            if (string.IsNullOrEmpty(referenceNamespace))
                return typeName;
            var ns2 = NamespaceAndName.Parse(typeName);
            if (referenceNamespace == ns2.Namespace)
                return ns2.Name;
            return typeName;
        }
    }
}