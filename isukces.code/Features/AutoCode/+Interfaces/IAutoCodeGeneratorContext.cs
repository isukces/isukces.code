using System;

namespace isukces.code.AutoCode
{
    public interface IAutoCodeGeneratorContext : IConfigResolver
    {
        void AddNamespace(string namepace);
        CsClass GetOrCreateClass(Type type);
    }

    public static class AutoCodeGeneratorContextExt
    {
        public static void AddNamespace(this IAutoCodeGeneratorContext src, Type type)
        {
            var ns = type.Namespace;
            if (!string.IsNullOrEmpty(ns))
                src.AddNamespace(ns);
        }
        
        public static void AddNamespace<T>(this IAutoCodeGeneratorContext src)
        {
            AddNamespace(src, typeof(T));
        }
    }
}