using System;
using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;

namespace isukces.code.vssolutions;

[ImmutableObject(true)]
public class PossibleToLoadNuget : IEquatable<PossibleToLoadNuget>, IComparable<PossibleToLoadNuget>, IComparable
{
    public PossibleToLoadNuget([NotNull] FrameworkVersion version, NugetLoadCompatibility compatibility)
    {
        Version       = version ?? throw new ArgumentNullException(nameof(version));
        Compatibility = compatibility;
    }

    public static bool operator ==(PossibleToLoadNuget left, PossibleToLoadNuget right)
    {
        return Equals(left, right);
    }

    public static bool operator >(PossibleToLoadNuget left, PossibleToLoadNuget right)
    {
        return Comparer<PossibleToLoadNuget>.Default.Compare(left, right) > 0;
    }

    public static bool operator >=(PossibleToLoadNuget left, PossibleToLoadNuget right)
    {
        return Comparer<PossibleToLoadNuget>.Default.Compare(left, right) >= 0;
    }

    public static bool operator !=(PossibleToLoadNuget left, PossibleToLoadNuget right)
    {
        return !Equals(left, right);
    }

    public static bool operator <(PossibleToLoadNuget left, PossibleToLoadNuget right)
    {
        return Comparer<PossibleToLoadNuget>.Default.Compare(left, right) < 0;
    }

    public static bool operator <=(PossibleToLoadNuget left, PossibleToLoadNuget right)
    {
        return Comparer<PossibleToLoadNuget>.Default.Compare(left, right) <= 0;
    }

    public int CompareTo(PossibleToLoadNuget other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        var compatibilityComparison = Compatibility.CompareTo(other.Compatibility);
        if (compatibilityComparison != 0) return compatibilityComparison;
        return Version.CompareTo(other.Version);
    }

    public int CompareTo(object obj)
    {
        if (ReferenceEquals(null, obj)) return 1;
        if (ReferenceEquals(this, obj)) return 0;
        return obj is PossibleToLoadNuget other
            ? CompareTo(other)
            : throw new ArgumentException($"Object must be of type {nameof(PossibleToLoadNuget)}");
    }

    public bool Equals(PossibleToLoadNuget other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(Version, other.Version) && Compatibility == other.Compatibility;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((PossibleToLoadNuget)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((Version != null ? Version.GetHashCode() : 0) * 397) ^ (int)Compatibility;
        }
    }

    public override string ToString()
    {
        return $"Version={Version}, Compatibility={Compatibility}";
    }

    public FrameworkVersion Version { get; }

    public NugetLoadCompatibility Compatibility { get; }
}