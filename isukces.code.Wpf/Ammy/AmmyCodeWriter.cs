using System.Collections.Generic;
using System.Linq;
using isukces.code.interfaces;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Wpf.Ammy
{
    public class AmmyCodeWriter : CodeWriter, IAmmyNamespaceProvider, IAmmyCodeWriter
    {
        public AmmyCodeWriter() : base(AmmyLangInfo.Instance)
        {
        }

        public void AddNamespace<T>()
        {
            _namespaces.Add(typeof(T).Namespace);
        }

      


        private List<string>          _namespaces { get; } = new List<string>();
        public  IReadOnlyList<string> Namespaces  => _namespaces;
    }
}