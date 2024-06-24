#nullable enable
using System;
using iSukces.Code.Interfaces;

namespace iSukces.Code;

public sealed class CodeDocumentationKey : IEquatable<CodeDocumentationKey>
{
    public bool Equals(CodeDocumentationKey? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Kind == other.Kind && Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj is CodeDocumentationKey key && Equals(key);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((int)Kind * 397) ^ Name.GetHashCode();
        }
    }

    public static bool operator ==(CodeDocumentationKey? left, CodeDocumentationKey? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(CodeDocumentationKey? left, CodeDocumentationKey? right)
    {
        return !Equals(left, right);
    }

    public override string ToString()
    {
        return Kind + " " + Name;
    }

    public CodeDocumentationKey(CodeDocumentationKind kind, string? name)
    {
        Kind = kind;
        Name = name ?? "";
    }

    public static CodeDocumentationKey? FromString(string? keyAsString)
    {
        keyAsString??=string.Empty;
        if (keyAsString.Length < 3)
            return null;
        var type      = keyAsString.Substring(0, 2);
        var shortName = keyAsString.Substring(2);
        var kind = type switch
        {
            "M:" => CodeDocumentationKind.Method,
            "P:" => CodeDocumentationKind.Property,
            "T:" => CodeDocumentationKind.Type,
            "F:" => CodeDocumentationKind.Field,
            _ => CodeDocumentationKind.Unknown
        };
        if (kind == CodeDocumentationKind.Unknown)
            return null;
        return new CodeDocumentationKey(kind, shortName);
    }

    public CodeDocumentationKind Kind { get; }
    public string Name { get; }
}