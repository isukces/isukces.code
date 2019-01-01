using System.Collections.Generic;
using JetBrains.Annotations;

namespace isukces.code.interfaces.Ammy
{
    public interface IAmmyNamespaceProvider
    {
        [NotNull]
        IReadOnlyList<string> Namespaces { get; }
    }
}