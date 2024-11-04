#nullable enable
using System;
using System.Linq;
using iSukces.Code.Interfaces;

namespace iSukces.Code.AutoCode;

public readonly struct TypeProvider : IEquatable<TypeProvider>
{
    public TypeProvider(Type? type, CsType typeName, CsNamespaceMemberKind kind)
    {
        Type     = type;
        Kind     = kind;
        TypeName = typeName;
    }

    public static TypeProvider FromType(Type type) => new(type, default, type.GetNamespaceMemberKind());

    public static TypeProvider FromTypeName(CsType typeName, CsNamespaceMemberKind kind)
        => new(null, typeName, kind);
    
    public static bool operator ==(TypeProvider left, TypeProvider right) => left.Equals(right);

    public static bool operator !=(TypeProvider left, TypeProvider right) => !left.Equals(right);

    public bool Equals(TypeProvider other) => Type == other.Type
                                              && TypeName.Equals(other.TypeName)
                                              && Kind == other.Kind;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is TypeProvider other && Equals(other);
    }


    public override int GetHashCode()
    {
        unchecked
        {
            var typeHash     = Type?.GetHashCode() ?? 0;
            var typeNameHash = TypeName.GetHashCode();
            return (typeHash * 397) ^ typeNameHash;
        }
    }

    public bool                  IsEmpty  => Type is null && TypeName.IsVoid;
    public Type?                 Type     { get; }
    public CsType                TypeName { get; }
    public CsNamespaceMemberKind Kind     { get; }
}
