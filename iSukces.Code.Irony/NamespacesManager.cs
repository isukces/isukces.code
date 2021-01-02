using System.Collections.Generic;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    internal class NamespacesManager
    {
        public void Apply(string fullClassName, CsNamespace csNamespace)
        {
            if (csNamespace is null)
                return;
            if (string.IsNullOrEmpty(fullClassName))
                return;
            var ns = NamespaceAndName.Parse(fullClassName).Namespace;
            if (string.IsNullOrEmpty(ns))
                return;
            if (!_dictionary.TryGetValue(ns, out var set))
                return;
            foreach (var i in set)
                csNamespace.AddImportNamespace(i);
        }

        public void ShouldSee(FullTypeName c1, FullTypeName c2)
        {
            if (c1 is null || c2 is null)
                return;
            var ns2 = NamespaceAndName.Parse(c2.Name).Namespace ?? string.Empty;
            if (string.IsNullOrEmpty(ns2))
                return;
            var ns1 = NamespaceAndName.Parse(c1.Name).Namespace ?? string.Empty;
            if (ns1 == ns2)
                return;
            if (!_dictionary.TryGetValue(ns1, out var set)) _dictionary[ns1] = set = new HashSet<string>();

            set.Add(ns2);
        }

        private readonly Dictionary<string, HashSet<string>> _dictionary
            = new Dictionary<string, HashSet<string>>();
    }
}