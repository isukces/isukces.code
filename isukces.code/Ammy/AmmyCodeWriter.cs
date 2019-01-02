using System.Collections.Generic;
using isukces.code.interfaces;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public class AmmyCodeWriter : CodeWriter, IAmmyNamespaceProvider, IAmmyCodeWriter
    {
        public AmmyCodeWriter() : base(AmmyLangInfo.Instance)
        {
        }

        public ISet<string> Namespaces => _namespaces;

        private readonly HashSet<string> _namespaces = new HashSet<string>();
    }
}