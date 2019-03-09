using System;

namespace isukces.code.interfaces
{
    /// <summary>
    ///     Represents type name with namespace not related to platform (CLR, typscript etc)
    /// </summary>
    public struct AbstractType
    {
        public AbstractType(Type clrType) :
            this(clrType.Namespace, clrType.Name)
        {
        }

        public AbstractType(string ns, string name)
        {
            Namespace = ns?.Trim() ?? string.Empty;
            Name      = name?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(name))
                throw new Exception("Invalid name");
        }

        public AbstractType MoveToNs(string ns)
        {
            return new AbstractType(ns, Name);
        }

        public override string ToString()
        {
            return FullName;
        }

        public AbstractType WithNameOverride(string newName)
        {
            if (string.IsNullOrEmpty(newName))
                return this;
            return new AbstractType(Namespace, newName);
        }

        public string Namespace { get; }
        public string Name      { get; }
        public string FullName  => string.IsNullOrEmpty(Namespace) ? Name : Namespace + "." + Name;
    }
}