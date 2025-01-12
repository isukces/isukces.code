using System;
using System.Collections.Generic;
using System.Linq;

namespace iSukces.Code.VsSolutions;

public struct TargetFramework
{
    public static TargetFramework operator +(TargetFramework a, string b)
    {
        if (string.IsNullOrWhiteSpace(b))
            return a;
        if (a.Count == 0)
            return b;
        return a.Text + ";" + b;
    }

    public static implicit operator TargetFramework(string? value)
    {
        var value1 = new TargetFramework
        {
            _list = value?.Split(';')
                .Select(a => a.Trim())
                .Distinct()
                .Where(a => !string.IsNullOrEmpty(a))
                .OrderBy(a => new FrameworkNameSorter(a))
                .ToList()
        };
        return value1;
    }

    public bool Contains(string value)
    {
        return _list?.Contains(value) == true;
    }

    public TargetFramework Remove(Predicate<string> x)
    {
        if (Count == 0)
            return this;
        var list = _list!.Where(a => !x(a)).ToList();
        return new TargetFramework { _list = list };
    }

    public override string ToString() => Text;

    public string Text => _list is null ? string.Empty : string.Join(";", _list);

    public int Count => _list?.Count ?? 0;

    public IReadOnlyList<string> List => _list ?? [];

    private List<string>? _list;
}

