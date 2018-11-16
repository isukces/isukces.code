using System;
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
}