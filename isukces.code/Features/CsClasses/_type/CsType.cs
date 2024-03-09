#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace iSukces.Code;

public struct CsType : IEquatable<CsType>, IComparable<CsType>, IComparable
{
    public CsType(string? type)
    {
        BaseName = type?.Trim();
        if (BaseName == "void")
            BaseName = null;
        _genericParamaters = null;
        _arrayRanks        = null;
    }

    public static bool operator ==(CsType left, CsType right) => left.Equals(right);

    public static explicit operator CsType(string type) => new CsType(type);

    public static bool operator >(CsType left, CsType right) => left.CompareTo(right) > 0;

    public static bool operator >=(CsType left, CsType right) => left.CompareTo(right) >= 0;

    public static bool operator !=(CsType left, CsType right) => !left.Equals(right);

    public static bool operator <(CsType left, CsType right) => left.CompareTo(right) < 0;

    public static bool operator <=(CsType left, CsType right) => left.CompareTo(right) <= 0;

    public CsType AppendBase(string append) => WithBaseName(BaseName + append);

    public string AsString(bool allowReferenceNullable)
        => IsVoid ? "void" : GetNotEmpty(allowReferenceNullable, false);

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
        return obj is CsType other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(CsType)}");
    }

    public bool Equals(CsType other)
    {
        if (IsVoid)
            return other.IsVoid;
        return Modern == other.Modern;
    }

    public override bool Equals(object? obj) => obj is CsType other && Equals(other);

    private string GetAsGenericParameter(bool allowReferenceNullable)
        => IsVoid ? "" : GetNotEmpty(allowReferenceNullable, false);

    public override int GetHashCode() => (BaseName != null ? BaseName.GetHashCode() : 0) * 397 + GenericParamaters.Count;

    public string GetMemberCode(string member) => $"{Declaration}.{member}";


    private string GetNotEmpty(bool allowReferenceNullable, bool ignoreGenerics)
    {
        var addQuestionMark =
            allowReferenceNullable && Nullable == NullableKind.ReferenceNullable
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

    public string New(string args) => $"new {Declaration}({args})";

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

    public override string ToString()
    {
        if (GlobalSettings.DoNotAllowCsTypeToString == InvalidOperationNotification.Ignore)
            return AsString(false);

        const string message = "Converting CsType to string is not allowed. Use AsString method instead.";

        if (GlobalSettings.DoNotAllowCsTypeToString == InvalidOperationNotification.ThrowException)
            throw new InvalidOperationException(message);

        if (GlobalSettings.DoNotAllowCsTypeToString == InvalidOperationNotification.EmergencyLog)
            GlobalSettings.EmergencyLog(message);

        return "(" + message + ")";
    }

    public CsType WithBaseName(string baseName) => new(baseName)
    {
        Nullable          = Nullable,
        GenericParamaters = GenericParamaters,
        ArrayRanks        = ArrayRanks
    };

    public CsType WithGenericParameter(CsType genericParameter)
    {
        _genericParamaters = new[] { genericParameter };
        return this;
    }

    public CsType WithReferenceNullable()
    {
        Nullable = NullableKind.ReferenceNullable;
        return this;
    }

    #region Properties

    public string Modern      => AsString(true);
    public string Declaration => AsString(false);

    public string? BaseName { get; }

    public bool IsVoid => string.IsNullOrEmpty(BaseName);

    public static CsType Void => new CsType(string.Empty);

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

    #endregion

    #region Fields

    public static readonly CsType Int32 = new CsType("int");

    public static readonly CsType NullableInt32 = new CsType("int")
    {
        Nullable = NullableKind.ValueNullable
    };

    public static readonly CsType String = new CsType("string");
    public static readonly CsType Object = new CsType("object");

    public static readonly CsType ObjectNullable = new CsType("object")
    {
        Nullable = NullableKind.ReferenceNullable
    };

    public static readonly CsType Bool = new CsType("bool");

    private IReadOnlyList<CsType>? _genericParamaters;
    private IReadOnlyList<Rank>? _arrayRanks;

    #endregion
}

public enum NullableKind
{
    Unknown,
    NotNull,
    ValueNullable,
    ReferenceNullable
}

public record struct Rank(int Number, bool ReferenceNullable);
