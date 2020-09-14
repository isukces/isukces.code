using System.Collections.Generic;
using iSukces.Code.Interfaces.Ammy;

namespace iSukces.Code.Ammy
{
    public class AmmyNamespaceProvider : IAmmyNamespaceProvider
    {
        public ISet<string> Namespaces { get; } = new HashSet<string>();
    }
}