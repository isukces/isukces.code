#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Interfaces;

namespace iSukces.Code;

public class NamespacesHolder : INamespaceContainer
{
    public NamespacesHolder(Func<string?, UsingInfo>? ownerCheckFirst)
    {
        _ownerCheckFirst = ownerCheckFirst ?? Empty;
    }

    public void Add(IEnumerable<string?> namespaces)
    {
        foreach (var ns in namespaces)
            if (!string.IsNullOrEmpty(ns))
                _namespaces.Add(ns!);
    }

    public void Add(string? ns, string? alias = null)
    {
        if (string.IsNullOrEmpty(ns)) 
            return;
        if (string.IsNullOrEmpty(alias))
            _namespaces.Add(ns!);
        else
            _aliases[ns!] = alias!;
    }

    public bool Emit(ICsCodeWriter writer, INamespaceContainer? except)
    {
        var ns = GetNamespacesForEmit(except).ToArray();
        if (ns.Length == 0)
            return false;
        foreach (var n in ns)
            writer.WriteLine($"using {n};");
        return true;
    }

    private UsingInfo Empty(string? ns) => new();

    public IReadOnlyList<string> GetNamespaces()
    {
        return _namespaces.OrderBy(a => a).ToArray();
    }

    private IEnumerable<string> GetNamespacesForEmit(INamespaceContainer? except)
    {
        foreach (var ns in _namespaces.OrderBy(a => a))
        {
            if (except is not null)
            {
                var info = except.GetNamespaceInfo(ns);
                if (info.IsKnownWithoutAlias())
                    continue;
            }

            if (_aliases.ContainsKey(ns))
                continue;
            yield return ns;
        }

        if (_aliases.Count != 0)
        {
            var pairs = _aliases
                .OrderBy(a => a.Key);
            foreach (var pair in pairs)
            {
                yield return $"{pair.Value} = {pair.Key}";
            }
        }

        if (_static.Count != 0)
            foreach (var ns in _static.OrderBy(a => a))
                yield return $"static {ns}";
    }

    public UsingInfo GetNamespaceInfo(string? namespaceName)
    {
        if (string.IsNullOrEmpty(namespaceName))
            return new UsingInfo(true);

        var q = _ownerCheckFirst(namespaceName);
        if (q.IsKnown)
            return q;

        if (_aliases.TryGetValue(namespaceName!, out var alias))
            return new UsingInfo(true, alias);
        if (_namespaces.Contains(namespaceName!))
            return new UsingInfo(true);
        return new UsingInfo(false);
    }

    #region Fields

    private readonly Dictionary<string, string> _aliases    = new();
    private readonly HashSet<string>            _namespaces = [];
    private readonly Func<string?, UsingInfo>   _ownerCheckFirst;
    private readonly HashSet<string>            _static = [];

    #endregion
}