using System;
using System.Collections.Generic;
using isukces.code.AutoCode;
using JetBrains.Annotations;

namespace isukces.code.interfaces.Ammy
{
    public interface IAmmyNamespaceProvider
    {
        [NotNull]
        ISet<string> Namespaces { get; }
    }

    public static class AmmyNamespaceProviderExt
    {
        public static void AddNamespace([NotNull] this IAmmyNamespaceProvider src, [NotNull] Type type)
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            if (type == null) throw new ArgumentNullException(nameof(type));

            var at = EmitTypeAttribute.GetAttribute(type);
            if (at?.Namespace is null)
            {
                var ns = type.Namespace;
                if (!string.IsNullOrEmpty(ns))
                    src.Namespaces.Add(ns);
            }
            else
            {
                src.Namespaces.Add(at.Namespace);
            }
        }

        public static void AddNamespace<T>([NotNull] this IAmmyNamespaceProvider src)
        {
            src.AddNamespace(typeof(T));
        }

        public static string GetTypeName(this IAmmyNamespaceProvider provider, Type type)
        {
            INamespaceContainer container = new MethodNamespaceContainer(provider?.Namespaces);
            return container.GetTypeName(type);
        }

        private class MethodNamespaceContainer : INamespaceContainer
        {
            public MethodNamespaceContainer(ISet<string> known) => _known = known ?? new HashSet<string>();

            public bool IsKnownNamespace(string namespaceName) => _known.Contains(namespaceName);

            private readonly ISet<string> _known;
        }
    }
}