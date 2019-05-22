using System;
using System.IO;
using System.Reflection;

namespace isukces.code.AutoCode
{
    public interface IAutoCodeGenerator
    {
        void Generate(Type type, IAutoCodeGeneratorContext context);
    }

    public interface IAssemblyAutoCodeGenerator
    {
        void AssemblyStart(Assembly assembly, IAutoCodeGeneratorContext context);
        void AssemblyEnd(Assembly assembly, IAutoCodeGeneratorContext context);
    }

    public interface IAssemblyBaseDirectoryProvider
    {
        DirectoryInfo GetBaseDirectory(Assembly assembly);
    }

    public interface IAssemblyFilenameProvider
    {
        FileInfo GetFilename(Assembly assembly);
    }
}