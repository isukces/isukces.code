#nullable enable
using System;

namespace iSukces.Code.Interfaces
{
    public struct NamespaceAndName : IEquatable<NamespaceAndName>
    {
        public NamespaceAndName(string? ns, string? name)
        {
            Namespace = ns?.Trim();
            Name      = name?.Trim();
            if (Namespace == string.Empty)
                Namespace = null;
            if (Name == string.Empty)
                Name = null;
        }

        public static NamespaceAndName FromType<T>()
        {
            return new NamespaceAndName(typeof(T).Namespace, typeof(T).Name);
        }

        public static NamespaceAndName FromType(Type t)
        {
            return new NamespaceAndName(t.Namespace, t.Name);
        }

        public static bool operator ==(NamespaceAndName left, NamespaceAndName right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(NamespaceAndName left, NamespaceAndName right)
        {
            return !left.Equals(right);
        }

        public static NamespaceAndName Parse(string fullClassName)
        {
            var idx = fullClassName.LastIndexOf('.');
            if (idx < 0)
                return new NamespaceAndName(null, fullClassName);
            return new NamespaceAndName(fullClassName.Substring(0, idx), fullClassName.Substring(idx + 1));
        }

        public bool Equals(NamespaceAndName other)
        {
            return Namespace == other.Namespace && Name == other.Name;
        }

        public override bool Equals(object? obj)
        {
            return obj is NamespaceAndName other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Namespace != null ? Namespace.GetHashCode() : 0) * 397) ^
                       (Name != null ? Name.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return FullName;
        }

        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(Name))
                    return Namespace;
                if (string.IsNullOrEmpty(Namespace))
                    return Name;
                return Namespace + "." + Name;
            }
        }

        public string Namespace { get; }
        public string Name      { get; }
    }
}
