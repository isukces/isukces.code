using System.Collections.Generic;
using System.Linq;
using isukces.code.interfaces;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Wpf.Ammy
{
    public class AmmyCodeWritter : CodeWritter, IAmmyNamespaceProvider, IAmmyCodeWritter
    {
        public AmmyCodeWritter() : base(AmmyLangInfo.Instance)
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