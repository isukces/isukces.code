using System;
using System.Collections.Generic;

namespace isukces.code.interfaces
{
    public interface INamespaceContainer
    {
        ISet<string> GetNamespaces(bool withParent);
    }

    public interface INamespaceCollection
    {
        void AddImportNamespace(string aNamespace);
    }

    public static class NamespaceCollectionExt
    {
        public static void AddImportNamespace(this INamespaceCollection self, params string[] namespaces)
        {
            if (namespaces == null || namespaces.Length == 0)
                return;
            for (var index = 0; index < namespaces.Length; index++)
            {
                var ns = namespaces[index];
                self.AddImportNamespace(ns);
            }
        }

        public static void AddImportNamespace(this INamespaceCollection self, params Type[] types)
        {
            if (types == null || types.Length == 0)
                return;
            for (var index = 0; index < types.Length; index++)
            {
                var ns = types[index];
                self.AddImportNamespace(ns);
            }
        }

        public static void AddImportNamespace(this INamespaceCollection self, Type type)
        {
            self.AddImportNamespace(type.Namespace);
        }
    }

    public interface ITypeNameResolver
    {
        string TypeName(Type type);
    }

    public interface IClassOwner : INamespaceContainer, ITypeNameResolver
    {
    }

    public interface INamespaceOwner : INamespaceContainer, ITypeNameResolver
    {
    }

    public interface IConditional
    {
        /// <summary>
        ///     Compiler directive required for element
        /// </summary>
        string CompilerDirective { get; set; }
    }

    public static class ConditionalExtensions
    {
        public static T WithCompilerDirective<T>(this T src, string directive)
            where T : IConditional
        {
            src.CompilerDirective = directive;
            return src;
        }
    }
}