using System;
using iSukces.Code.Interfaces;

namespace iSukces.Code.AutoCode
{
    public struct TypeProvider : IEquatable<TypeProvider>
    {
        public TypeProvider(Type type, string typeName, CsNamespaceMemberKind kind)
        {
            Type     = type;
            Kind     = kind;
            TypeName = typeName?.Trim();
        }

        public static TypeProvider FromType(Type type)
        {
            return new TypeProvider(type, null, type.GetNamespaceMemberKind());
        }

        public static TypeProvider FromTypeName(string typeName, CsNamespaceMemberKind kind)
        {
            return new TypeProvider(null, typeName, kind);
        }

        public static bool operator ==(TypeProvider left, TypeProvider right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TypeProvider left, TypeProvider right)
        {
            return !left.Equals(right);
        }

        public bool Equals(TypeProvider other)
        {
            return Type == other.Type
                   && string.Equals(TypeName ?? string.Empty, other.TypeName ?? string.Empty)
                   && Kind == other.Kind;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TypeProvider other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var typeHash     = Type?.GetHashCode() ?? 0;
                var typeNameHash = (TypeName ?? string.Empty).GetHashCode();
                return (typeHash * 397) ^ typeNameHash;
            }
        }

        public bool IsEmpty => Type == null && string.IsNullOrEmpty(TypeName);

        public Type                  Type     { get; }
        public string                TypeName { get; }
        public CsNamespaceMemberKind Kind     { get; }
    }
}