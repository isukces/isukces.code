using System;

namespace isukces.code.AutoCode
{
    public interface IAutoCodeGeneratorContext : IConfigResolver
    {
        void AddNamespace(string namepace);
        CsClass GetOrCreateClass(Type type);
    }
}