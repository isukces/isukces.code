#nullable enable
using System;

namespace iSukces.Code.FeatureImplementers;

public static partial class CommonKeyType
{
    public static CsType ToNetType(this Kind type)
    {
        return type switch
        {
            Kind.Int => CsType.Int32,
            Kind.Guid => CsType.Guid,
            Kind.Long => (CsType)"long",
            Kind.String => CsType.String,
            Kind.ULong => (CsType)"ulong",
            _ => throw new NotSupportedException(type.ToString())
        };
    }

    public enum Kind
    {
        Int,
        Guid,
        Long,
        ULong,
        String,
    }
}
