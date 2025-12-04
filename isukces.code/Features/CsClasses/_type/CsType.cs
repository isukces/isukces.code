using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using iSukces.Code.Interfaces;

namespace iSukces.Code;

public struct CsType
// public class CsType
    : IEquatable<CsType>, IComparable<CsType>, IComparable
{
    public CsType(string? namespaceOrAlias, string? type)
        :this(type)
    {
        if (BaseName == "void" || string.IsNullOrEmpty(namespaceOrAlias))
            return;
        BaseName = $"{namespaceOrAlias}.{BaseName}";
    }
    public CsType(string? type)
    {
        BaseName = type?.Trim();
        if (BaseName == "void")
            BaseName = null;
        _genericParamaters = null;
        _arrayRanks        = null;
    }

    public CsType(string? type, NullableKind nullable)
    {
        BaseName = type?.Trim();
        if (BaseName == "void")
            BaseName = null;
        _genericParamaters = null;
        _arrayRanks        = null;
        Nullable           = nullable;
    }

    public static UsingInfo MakeDefault(string typeNamespace)
    {
        return string.IsNullOrEmpty(typeNamespace) ? new(NamespaceSearchResult.Empty) : new(NamespaceSearchResult.NotFound);
    }
    
    public static CsType Generic(string name, CsType genericArgument)
    {
        return new CsType(name)
        {
            GenericParamaters = [genericArgument]
        };
    }

    public static CsType Generic(string name, string genericArgument)
    {
        return new CsType(name)
        {
            GenericParamaters = [new CsType(genericArgument)]
        };
    }

    public static CsType Generic(string name, CsType genericArgument1, CsType genericArgument2)
    {
        return new CsType(name)
        {
            GenericParamaters = [genericArgument1, genericArgument2]
        };
    }

    public static bool operator ==(CsType left, CsType right)
    {
        return left.Equals(right);
    }

    public static explicit operator CsType(string type)
    {
        return new CsType(type);
    }

    public static bool operator >(CsType left, CsType right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(CsType left, CsType right)
    {
        return left.CompareTo(right) >= 0;
    }

    public static bool operator !=(CsType left, CsType right)
    {
        return !left.Equals(right);
    }

    public static bool operator <(CsType left, CsType right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(CsType left, CsType right)
    {
        return left.CompareTo(right) <= 0;
    }

    public CsType AppendBase(string append)
    {
        return WithBaseName(BaseName + append);
    }

    public string AsString(bool allowReferenceNullable)
    {
        return IsVoid ? "void" : GetNotEmpty(allowReferenceNullable, false);
    }

    public int CompareTo(CsType other)
    {
        var c = string.Compare(BaseName, other.BaseName, StringComparison.Ordinal);
        if (c != 0)
            return c;
        c = string.Compare(Modern, other.Modern, StringComparison.Ordinal);
        return c;
    }

    public int CompareTo(object? obj)
    {
        if (ReferenceEquals(null, obj)) return 1;
        return obj is CsType other
            ? CompareTo(other)
            : throw new ArgumentException($"Object must be of type {nameof(CsType)}");
    }

    public bool Equals(CsType other)
    {
        if (IsVoid)
            return other.IsVoid;
        return Modern == other.Modern;
    }

    public override bool Equals(object? obj)
    {
        return obj is CsType other && Equals(other);
    }

    private string GetAsGenericParameter(bool allowReferenceNullable)
    {
        return IsVoid ? "" : GetNotEmpty(allowReferenceNullable, false);
    }

    public override int GetHashCode()
    {
        return (BaseName is not null ? BaseName.GetHashCode() : 0) * 397 + GenericParamaters.Count;
    }

    public string GetMemberCode(string member)
    {
        return $"{Declaration}.{member}";
    }


    private string GetNotEmpty(bool allowReferenceNullable, bool ignoreGenerics)
    {
        var addQuestionMark =
            (allowReferenceNullable && Nullable == NullableKind.ReferenceNullable)
            ||
            Nullable == NullableKind.ValueNullable;

        var array = "";
        if (ArrayRanks.Count > 0)
            array = string.Join("", ArrayRanks.Select(a =>
            {
                var brackets = $"[{new string(',', a.Number - 1)}]";
                if (a.ReferenceNullable && allowReferenceNullable)
                    return brackets + "?";
                return brackets;
            }));

        var g = GenericParamaters;
        if (g.Count == 0)
        {
            var nn = addQuestionMark ? $"{BaseName}?" : BaseName!;
            return nn + array;
        }

        var n = BaseName + GenericParamaters
            .Select(a => a.GetAsGenericParameter(allowReferenceNullable))
            .CommaJoin().TriangleBrackets();
        n += array;
        return addQuestionMark ? $"{n}?" : n;
    }

    public string GetTypeNameOrThrowIfVoid(bool allowReferenceNullable)
    {
        if (IsVoid)
            throw new InvalidOperationException("Method result type is void");
        return AsString(allowReferenceNullable);
    }

    public CsType MakeArray(int arrayRank = 1, bool referenceNullable = false)
    {
        var cnt    = ArrayRanks.Count;
        var append = new Rank(arrayRank, referenceNullable);

        var clone = (CsType)MemberwiseClone();
        if (cnt == 0)
        {
            clone._arrayRanks = new[] { append };
            return clone;
        }

        var l = new List<Rank>(cnt + 1);
        l.AddRange(ArrayRanks);
        l.Add(append);
        clone._arrayRanks = l;
        return clone;
    }

    public string New(string? args)
    {
        return $"new {Declaration}({args})";
    }

    public string New()
    {
        return $"new {Declaration}()";
    }

    public string New(string a1, string a2)
    {
        return New(a1 + GlobalSettings.CommaSeparator + a2);
    }

    public (string namespaceName, string shortClassName) SpitNamespaceAndShortName()
    {
        var typeNameParts  = BaseName!.Split('.');
        var namespaceParts = typeNameParts.Take(typeNameParts.Length - 1).ToArray();
        var namespaceName  = string.Join(".", namespaceParts);
        return (namespaceName, typeNameParts.Last());
    }

    public CsType StripNullableValue()
    {
        if (Nullable == NullableKind.ReferenceNullable)
            return this;
        var clone = (CsType)MemberwiseClone();
        if (Nullable == NullableKind.ValueNullable)
            clone.Nullable = NullableKind.NotNull;
        return clone;
    }
    
    public void ThrowIfArray()
    {
        if (ArrayRanks.Count > 0)
            throw new InvalidOperationException("Array type is not allowed");
    }

    public void ThrowIfVoid()
    {
        if (IsVoid)
            throw new InvalidOperationException("Void type is not allowed");
    }

    public string ThrowNew(string? args)
    {
        return $"throw new {Declaration}({args})";
    }

    public CsType ToReferenceNullableIfPossible(Type type)
    {
        if (!type.IsClass && !type.IsInterface) return this;
        var pt = this;
        pt.Nullable = NullableKind.ReferenceNullable;
        return pt;
    }

    public CsType ToReferenceNullableIfPossible(CsNamespaceMemberKind kind)
    {
        if (kind is not (CsNamespaceMemberKind.Class or CsNamespaceMemberKind.Interface
            or CsNamespaceMemberKind.Record)) return this;
        var pt = this;
        pt.Nullable = NullableKind.ReferenceNullable;
        return pt;
    }

    public override string ToString()
    {
        if (GlobalSettings.DoNotAllowCsTypeToString == InvalidOperationNotification.Ignore)
            return AsString(false);

        const string message = "Converting CsType to string is not allowed. Use AsString method instead.";

        switch (GlobalSettings.DoNotAllowCsTypeToString)
        {
            case InvalidOperationNotification.ThrowException:
                throw new InvalidOperationException(message);
            case InvalidOperationNotification.EmergencyLog:
                GlobalSettings.EmergencyLog(message);
                return Declaration;
            default:
                return $"({message})";
        }
    }

    public string TypeOf()
    {
        return $"typeof({Declaration})";
    }

    public CsType WithBaseName(string baseName)
    {
        return new CsType(baseName)
        {
            Nullable          = Nullable,
            GenericParamaters = GenericParamaters,
            ArrayRanks        = ArrayRanks
        };
    }

    public CsType WithGenericParameter(CsType genericParameter)
    {
        _genericParamaters = new[] { genericParameter };
        return this;
    }

    public CsType WithGenericParameters(CsType genericParameter1, CsType genericParameter2)
    {
        _genericParamaters = new[] { genericParameter1, genericParameter2 };
        return this;
    }

    public CsType WithGenericParameters(string genericParameter1, string genericParameter2)
    {
        _genericParamaters = new[] { (CsType)genericParameter1, (CsType)genericParameter2 };
        return this;
    }

    public CsType WithReferenceNullable()
    {
        var copy = this with
        {
            Nullable = NullableKind.ReferenceNullable
        };
        return copy;
    }

    public string  Modern      => AsString(true);
    public string  Declaration => AsString(false);
    public string? BaseName    { get; }

    public bool IsVoid => string.IsNullOrEmpty(BaseName);

    public static CsType Void => new(string.Empty);

    public IReadOnlyList<CsType> GenericParamaters
    {
        readonly get => _genericParamaters ?? Array.Empty<CsType>();
        set => _genericParamaters = value;
    }

    public NullableKind Nullable { get; set; }

    public IReadOnlyList<Rank> ArrayRanks
    {
        readonly get => _arrayRanks ?? Array.Empty<Rank>();
        set => _arrayRanks = value;
    }

    public string Constructor
        => IsVoid
            ? throw new InvalidOperationException("Void type has no constructor")
            : GetNotEmpty(false, true);

    public static readonly CsType Int32 = new("int");

    public static readonly CsType Int32Nullable = new("int", NullableKind.ValueNullable);

    public static readonly CsType Decimal = new("decimal");

    public static readonly CsType DecimalNullable = new("decimal", NullableKind.ValueNullable);

    public static readonly CsType Double = new("double");

    public static readonly CsType DoubleNullable = new("double", NullableKind.ValueNullable);

    public static readonly CsType Int64 = new("long");

    public static readonly CsType Int64Nullable = new("long", NullableKind.ValueNullable);

    public static readonly CsType Guid = new("System.Guid");

    public static readonly CsType String = new("string");

    public static readonly CsType StringNullable = new("string", NullableKind.ReferenceNullable);

    public static readonly CsType Object = new("object");

    public static readonly CsType ObjectNullable = new("object", NullableKind.ReferenceNullable);

    public static readonly CsType Bool = new("bool");

    public static readonly CsType BoolNullable = new("bool", NullableKind.ValueNullable);

    private IReadOnlyList<Rank>? _arrayRanks;

    private IReadOnlyList<CsType>? _genericParamaters;

}

public enum NullableKind
{
    Unknown,
    NotNull,
    ValueNullable,
    ReferenceNullable
}

public record struct Rank(int Number, bool ReferenceNullable);
