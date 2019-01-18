using System;
using isukces.code.interfaces;

namespace isukces.code.AutoCode
{
    public interface IAutoCodeGeneratorContext : IConfigResolver
    {
        void AddNamespace(string namepace);
        CsClass GetOrCreateClass(TypeProvider type);
    }

    public static class AutoCodeGeneratorContextExtensions
    {
        public static CsClass GetOrCreateClass(this IAutoCodeGeneratorContext self, Type type)
        {
            return self.GetOrCreateClass(TypeProvider.FromType(type));
        }

        public static CsClass GetOrCreateClass(this IAutoCodeGeneratorContext self, string typeName,
            CsNamespaceMemberKind kind)
        {
            return self.GetOrCreateClass(TypeProvider.FromTypeName(typeName, kind));
        }
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