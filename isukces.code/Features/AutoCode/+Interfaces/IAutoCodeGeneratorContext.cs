using System;
using System.Collections.Generic;
using System.IO;
using isukces.code.interfaces;
using JetBrains.Annotations;

namespace isukces.code.AutoCode
{
    public interface IAutoCodeGeneratorContext
    {
        void AddNamespace(string namepace);

        void FileSaved(FileInfo fileInfo);
        CsClass GetOrCreateClass(TypeProvider type);

        CsNamespace GetOrCreateNamespace(string namespaceName);

        [NotNull]
        IList<object> Tags { get; }
    }

    public static class AutoCodeGeneratorContextExtensions
    {
        public static CsClass GetOrCreateClass(this IAutoCodeGeneratorContext self, Type type) =>
            self.GetOrCreateClass(TypeProvider.FromType(type));

        public static CsClass GetOrCreateClass(this IAutoCodeGeneratorContext self, string typeName,
            CsNamespaceMemberKind kind) =>
            self.GetOrCreateClass(TypeProvider.FromTypeName(typeName, kind));
    }

    public static class AutoCodeGeneratorContextExt
    {
        public static void AddNamespace(this IAutoCodeGeneratorContext src, Type type)
        {
            var at = EmitTypeAttribute.GetAttribute(type);
            if (at?.Namespace is null)
            {
                var ns = type.Namespace;
                if (!string.IsNullOrEmpty(ns))
                    src.AddNamespace(ns);
            }
            else
            {
                src.AddNamespace(at.Namespace);
            }
        }

        public static void AddNamespace<T>(this IAutoCodeGeneratorContext src)
        {
            AddNamespace(src, typeof(T));
        }
    }
}