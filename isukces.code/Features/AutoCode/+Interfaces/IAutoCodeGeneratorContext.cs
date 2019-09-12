using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using isukces.code.interfaces;
using JetBrains.Annotations;

namespace isukces.code.AutoCode
{
    public interface IAutoCodeGeneratorContext : IConfigResolver
    {
        void AddNamespace(string namepace);
        CsClass GetOrCreateClass(TypeProvider type);
        
        [NotNull]
        IList<object> Tags { get; }

        void FileSaved(FileInfo fileInfo);
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
            var at = EmitTypeAttribute.GetAttribute(type);
            if (at?.Namespace is null)
            {
                var ns = type.Namespace;
                if (!string.IsNullOrEmpty(ns))
                    src.AddNamespace(ns);
            }
            else
                src.AddNamespace(at.Namespace);
        }

        public static void AddNamespace<T>(this IAutoCodeGeneratorContext src)
        {
            AddNamespace(src, typeof(T));
        }
    }
}