using System;
using System.IO;
using JetBrains.Annotations;

namespace iSukces.Code.vssolutions
{
    public class FileName : IEquatable<FileName>, IComparable<FileName>
    {
        public FileName([NotNull] FileInfo fileInfo)
            : this(fileInfo.FullName)
        {
        }

        private FileName(string fullName)
        {
            FullName = fullName;
#if PLATFORM_UNIX
            _hash = _fullName.GetHashCode();
#else
            _nameForCompare = FullName.ToLowerInvariant();
            _hash           = _nameForCompare.GetHashCode();
#endif
        }

        public static bool operator ==(FileName left, FileName right) => Equals(left, right);

        // Public Methods 

        public static bool operator !=(FileName left, FileName right) => !Equals(left, right);

        // Public Methods 

        public int CompareTo(FileName other)
        {
            if (Equals(other))
                return 0;
            if (other == null)
                return -1;
#if PLATFORM_UNIX
            return _fullName.CompareTo(other._fullName);
#else
            return _nameForCompare.CompareTo(other._nameForCompare);
#endif
        }

        public bool Equals(FileName other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
#if PLATFORM_UNIX
            return string.Equals(_fullName, other._fullName);
#else
            return string.Equals(_nameForCompare, other._nameForCompare);
#endif
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((FileName)obj);
        }

        public override int GetHashCode() => _hash;

        public override string ToString() => FullName;

        public string FullName { get; }

        public string Name
        {
            get { return new FileInfo(FullName).Name; }
        }

        public bool Exists
        {
            get { return File.Exists(FullName); }
        }

        public DirectoryInfo Directory
        {
            get { return new FileInfo(FullName).Directory; }
        }
#if !PLATFORM_UNIX
        private readonly string _nameForCompare;
#endif

        private readonly int _hash;
    }
}