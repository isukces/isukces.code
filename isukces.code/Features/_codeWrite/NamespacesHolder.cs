#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using iSukces.Code.AutoCode;
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
                _namespaces.Add(ns);
    }

    public void Add(string? ns, string? alias = null)
    {
        if (string.IsNullOrEmpty(ns))
            return;
        if (string.IsNullOrEmpty(alias))
            _namespaces.Add(ns);
        else
            _aliases[ns] = alias;
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

    private UsingInfo Empty(string? ns) => new(string.IsNullOrEmpty(ns) ? NamespaceSearchResult.Empty : NamespaceSearchResult.NotFound);

    public IReadOnlyList<string> GetNamespaces()
    {
        return _namespaces.OrderBy(a => a).ToArray();
    }

    public IEnumerable<string> GetNamespacesForEmit(INamespaceContainer? except)
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

        if (_typeAliases.Count != 0)
        {
            var pairs = _typeAliases
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

    public void AddTypeAlias(string alias, string typeName)
    {
        _typeAliases[typeName] = alias;
    }
    public void AddTypeAlias(string alias, Type type)
    {
        var typeName = GeneratorsHelper.GetTypeName(FullNameTypeNameResolver.Instance, type);
        _typeAliases[typeName.Declaration] = alias;
    }
    
    public UsingInfo GetNamespaceInfo(string? namespaceName)
    {
        if (string.IsNullOrEmpty(namespaceName))
            return new UsingInfo(NamespaceSearchResult.Empty);

        var q = _ownerCheckFirst(namespaceName);
        if (q.SearchResult != NamespaceSearchResult.NotFound)
            return q;

        if (_aliases.TryGetValue(namespaceName, out var alias))
            return new UsingInfo(NamespaceSearchResult.Found, alias);
        if (_namespaces.Contains(namespaceName))
            return new UsingInfo(NamespaceSearchResult.Found);
        return new UsingInfo(NamespaceSearchResult.NotFound);
    }

    public string? TryGetTypeAlias(TypeProvider type)
    {
        if (type.Type is not null)
        {
            var typeName = GeneratorsHelper.GetTypeName(FullNameTypeNameResolver.Instance, type.Type);
#if NETSTANDARD2_0            
            return _typeAliases.GetValueOrDefault(typeName.Declaration);
#else
            return _typeAliases.TryGetValue(typeName.Declaration, out var alias) 
                ? alias 
                : null;
#endif
            
        }

        // todo: handle other cases
        return null;
    }

    private readonly Dictionary<string, string> _aliases     = new();
    private readonly Dictionary<string, string> _typeAliases = new();
    private readonly HashSet<string>            _namespaces  = [];
    private readonly Func<string?, UsingInfo>   _ownerCheckFirst;
    private readonly HashSet<string>            _static = [];
}