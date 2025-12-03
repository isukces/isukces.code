using System;

namespace iSukces.Code.Interfaces;

public static class NamespaceCollectionExtensions
{
    extension(INamespaceCollection self)
    {
        public void AddImportNamespace(params string[]? namespaces)
        {
            if (namespaces is null || namespaces.Length == 0)
                return;
            for (var index = 0; index < namespaces.Length; index++)
            {
                var ns = namespaces[index];
                self.AddImportNamespace(ns);
            }
        }

        public void AddImportNamespace(params Type[]? types)
        {
            if (types is null || types.Length == 0)
                return;
            for (var index = 0; index < types.Length; index++)
            {
                var ns = types[index];
                self.AddImportNamespace(ns);
            }
        }

        public void AddImportNamespace(Type type, string? alias = null)
        {
            var ns = type.Namespace;
            if (!string.IsNullOrEmpty(ns))
                self.AddImportNamespace(type.Namespace, alias);
        }

        public void AddImportNamespace<T>(string? alias = null)
        {
            AddImportNamespace(self, typeof(T), alias);
        }
    }
}
