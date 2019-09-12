using System;
using System.Collections.Generic;
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
            
            var ns = type.Namespace;
            if (!string.IsNullOrEmpty(ns))
                src.Namespaces.Add(ns);
        }

        public static void AddNamespace<T>([NotNull] this IAmmyNamespaceProvider src)
        {
            src.AddNamespace(typeof(T));
        }
    }
}