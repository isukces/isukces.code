using System;
using System.Collections.Generic;
using iSukces.Code.Ammy;
using iSukces.Code.AutoCode;
using JetBrains.Annotations;

namespace iSukces.Code.Interfaces.Ammy
{
    public interface IAmmyNamespaceProvider
    {
        [NotNull]
        ISet<string> Namespaces { get; }
    }

    public static class AmmyNamespaceProviderExt
    {
        public static void AddNamespace([NotNull] this IAmmyNamespaceProvider src, [NotNull] Type type, int scanUpBaseClas = 0)
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            if (type == null) throw new ArgumentNullException(nameof(type));

            while (type != null)
            {
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

                if (scanUpBaseClas-- < 1)
                    break;
                type = type.BaseType;

            }
        }

        public static void AddNamespace<T>([NotNull] this IAmmyNamespaceProvider src, int scanUpBaseClas = 0)
        {
            src.AddNamespace(typeof(T), scanUpBaseClas);
        }

        public static string GetTypeName(this IAmmyNamespaceProvider provider, Type type)
        {
            if (AmmyGlobals.Instance.TryResolveTypeName(type, out var name))
                return name;
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