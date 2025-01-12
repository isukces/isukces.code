#nullable disable
using System;
using System.IO;
using JetBrains.Annotations;

namespace iSukces.Code.VsSolutions;

public class FileName : IEquatable<FileName>, IComparable<FileName>
{
    public FileName([NotNull] FileInfo fileInfo)
        : this(fileInfo.FullName)
    {
    }

    private FileName(string fullName)
    {
        FullName = fullName;
        _hash    = FileNameComparer.GetHashCode(fullName);
    }

    public static bool operator ==(FileName left, FileName right)
    {
        return Equals(left, right);
    }


    public static bool operator !=(FileName left, FileName right)
    {
        return !Equals(left, right);
    }


    public int CompareTo(FileName other)
    {
        if (Equals(other))
            return 0;
        if (other == null)
            return -1;
        return FileNameComparer.Compare(FullName, other.FullName);
    }

    public bool Equals(FileName other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return FileNameComparer.Equals(FullName, other.FullName);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((FileName)obj);
    }

    public override int GetHashCode()
    {
        return _hash;
    }

    public override string ToString()
    {
        return FullName;
    }

    public string FullName { get; }

    public string Name => GetFileInfo().Name;

    public bool Exists => File.Exists(FullName);

    public DirectoryInfo Directory => new FileInfo(FullName).Directory;


#if PLATFORM_UNIX
        public static StringComparer FileNameComparer = StringComparer.Ordinal;
#else
    public static StringComparer FileNameComparer = StringComparer.OrdinalIgnoreCase;
#endif

    private readonly int _hash;

    public FileInfo GetFileInfo()
    {
        return new FileInfo(FullName);
    }
}
