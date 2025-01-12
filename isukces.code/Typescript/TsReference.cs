using System;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Typescript;

public sealed class TsReference : ITsCodeProvider, IEquatable<TsReference>
{
    static TsReference()
    {
        // #warning Only WINDOWS
        PathComparer = StringComparer.OrdinalIgnoreCase;
    }

    public TsReference(string path)
    {
        Path = path ?? throw new ArgumentNullException(nameof(path));
    }

    public static bool operator ==(TsReference? left, TsReference? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(TsReference? left, TsReference? right)
    {
        return !Equals(left, right);
    }

    public bool Equals(TsReference? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return PathComparer.Equals(Path, other.Path);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((TsReference)obj);
    }

    public override int GetHashCode()
    {
        return PathComparer.GetHashCode(Path);
    }

    public override string ToString()
    {
        return "TsReference: " + Path;
    }

    public void WriteCodeTo(ITsCodeWriter writer)
    {
        writer.WriteLine($"/// <reference path=\"{Path}\"/>");
    }


    public static StringComparer PathComparer { get; }

    public string Path { get; }

}
