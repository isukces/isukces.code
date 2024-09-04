#nullable enable
using System;
using System.Reflection;
using iSukces.Code.AutoCode;

namespace iSukces.Code.Interfaces;

public interface IMemberNullValueChecker
{
    bool ReturnValueAlwaysNotNull(MemberInfo mi);
    bool TypeIsAlwaysNotNull(Type type);
}

public abstract class AbstractMemberNullValueChecker : IMemberNullValueChecker
{
    public bool ReturnValueAlwaysNotNull(MemberInfo mi)
    {
        var type = GeneratorsHelper.GetMemberResultType(mi);
        return TypeIsAlwaysNotNull(type) || HasMemberNotNullAttribute(mi);
    }

    protected abstract bool HasMemberNotNullAttribute(MemberInfo mi);

    public virtual bool TypeIsAlwaysNotNull(Type type)
    {
        var typeInfo = type.GetTypeInfo();
        if (typeInfo.IsClass || typeInfo.IsInterface)
            return false;
        if (!typeInfo.IsGenericType) return true;
        var gtt = type.GetGenericTypeDefinition();
        return gtt != typeof(Nullable<>);
    }
}