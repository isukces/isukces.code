using System;
using System.IO;
using System.Reflection;

namespace iSukces.Code.AutoCode
{
    public interface IAutoCodeGeneratorBase
    {
        
    }
    public interface IAutoCodeGenerator : IAutoCodeGeneratorBase
    {
        void Generate(Type type, IAutoCodeGeneratorContext? context);
    }

    public interface IAssemblyAutoCodeGenerator : IAutoCodeGeneratorBase
    {
        void AssemblyEnd(Assembly assembly, IAutoCodeGeneratorContext? context);
        void AssemblyStart(Assembly assembly, IAutoCodeGeneratorContext? context);
    }

    public interface IAssemblyBaseDirectoryProvider
    {
        DirectoryInfo GetBaseDirectory(Assembly assembly);
    }

    public static class AssemblyBaseDirectoryProviderExt
    {
        public static DirectoryInfo GetBaseDirectory<T>(this IAssemblyBaseDirectoryProvider directoryProvider)
        {
            return directoryProvider.GetBaseDirectory(typeof(T)
#if COREFX
                .GetTypeInfo()
#endif
                .Assembly);
        }

        public static string GetFileName(this IAssemblyBaseDirectoryProvider directoryProvider,
            Assembly assembly,
            params string[]? items)
        {
            var dir = directoryProvider.GetBaseDirectory(assembly);
            if (items is null || items.Length == 0)
                return dir.FullName;
            var pathItems = new string[items.Length + 1];
            pathItems[0] = dir.FullName;
            Array.Copy(items, 0, pathItems, 1, items.Length);
            var fn = Path.Combine(pathItems);
            return fn;
        }

        public static string GetFileName<T>(this IAssemblyBaseDirectoryProvider directoryProvider,
            params string[] items)
        {
            return directoryProvider.GetFileName(typeof(T)
#if COREFX
                .GetTypeInfo()
#endif
                .Assembly, items);
        }
    }

    public interface IAssemblyFilenameProvider
    {
        FileInfo GetFilename(Assembly assembly);
    }
}
