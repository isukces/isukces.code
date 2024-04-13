using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Interfaces;
using JetBrains.Annotations;

namespace iSukces.Code;

public class NamespacesHolder: INamespaceContainer
{
    public void Add(IEnumerable<string> namespaces)
    {
        foreach (var ns in namespaces) _namespaces.Add(ns);
    }

    public void Add(string ns)
    {
        _namespaces.Add(ns);
    }

    public IReadOnlyList<string> GetNamespaces()
    {
        return _namespaces.OrderBy(a => a).ToArray();
    }

    public IReadOnlyList<string> GetNamespaces([CanBeNull] INamespaceContainer except)
    {
        if (except is null)
            return GetNamespaces();
        return _namespaces
            .Where(a => !except.IsKnownNamespace(a))
            .OrderBy(a => a).ToArray();
    }

    public bool IsKnownNamespace(string namespaceName)
    {
        return !string.IsNullOrEmpty(namespaceName)
               && _namespaces.Contains(namespaceName);
    }

    private readonly HashSet<string> _namespaces = [];
}