using System.Collections.Generic;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public class AmmyNamespaceProvider : IAmmyNamespaceProvider
    {
        public ISet<string> Namespaces { get; } = new HashSet<string>();
    }
}