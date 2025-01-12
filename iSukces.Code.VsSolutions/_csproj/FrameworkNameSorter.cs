using System;
using System.Text.RegularExpressions;

namespace iSukces.Code.VsSolutions;

public struct FrameworkNameSorter : IComparable<FrameworkNameSorter>, IComparable
{
    public FrameworkNameSorter(string s)
    {
        var m = SplitRegex.Match(s);
        if (!m.Success)
            throw new ArgumentException("Invalid target framework " + s);

        this.Prefix  = m.Groups[1].Value.ToLower();
        this.Version = m.Groups[2].Value;
        this.Suffix  = m.Groups[3].Value.ToLower();
            
    }

    public int Group
    {
        get
        {
            // net48;net6.0;net8.0;net9.0;netcoreapp3.1
            if (Prefix == "net") return Version.Contains('.') ? 4 : 1;
            if (Prefix == "netcoreapp")
                return 2;
            if (Prefix == "netstandard")
                return 3;
            return 5;
        }
    }
    public int CompareTo(FrameworkNameSorter other)
    {
        var a = Group.CompareTo(other.Group);
        if (a != 0) return a;
        a = string.Compare(Version, other.Version, StringComparison.OrdinalIgnoreCase);
        if (a != 0) return a;
        var b = string.Compare(Prefix, other.Prefix, StringComparison.OrdinalIgnoreCase);
        if (b != 0) return b;
        return string.Compare(Suffix, other.Suffix, StringComparison.OrdinalIgnoreCase);
    }

    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        return obj is FrameworkNameSorter other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(FrameworkNameSorter)}");
    }

    public string Suffix { get; }

    public string Version { get;  }

    public string Prefix { get; }

    const string SplitFilter = @"^([^\d]+)(\d[^-]+)(-.*)?";
    static Regex SplitRegex = new Regex(SplitFilter, RegexOptions.Multiline | RegexOptions.Compiled);
        
}

