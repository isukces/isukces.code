using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace iSukces.Code.Interfaces
{
    public interface INamespaceContainer
    {
        bool IsKnownNamespace([CanBeNull]string namespaceName);    
    }

    public interface INamespaceCollection
    {
        void AddImportNamespace(string ns);
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
    
    public interface IClassOwner : INamespaceContainer, ITypeNameResolver
    {
    }
    
    public interface IEnumOwner : INamespaceContainer, ITypeNameResolver
    {
        /// <summary>
        ///     Enums
        /// </summary>
        IReadOnlyList<CsEnum> Enums { get;  }

        CsEnum AddEnum(CsEnum csEnum);
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