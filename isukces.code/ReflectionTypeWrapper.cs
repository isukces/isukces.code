using System;

namespace iSukces.Code;

public struct ReflectionTypeWrapper
{
    public ReflectionTypeWrapper(Type type)
    {
        _typeInfo = type;
        Type      = type;
    }

    public Type[] GetGenericArguments() => _typeInfo.GetGenericArguments();

    public Type GetGenericTypeDefinition() => _typeInfo.GetGenericTypeDefinition();

    public Type MakeNullableIfPossible()
    {
        if (IsValueType)
            return typeof(Nullable<>).MakeGenericType(Type);
        return Type;
    }

    public Type? UnwrapNullable(bool nullIfNotNullable = false)
    {
        if (Type is null) return null;
        if (!_typeInfo.IsGenericType) return nullIfNotNullable ? null : Type;
        var gt = _typeInfo.GetGenericTypeDefinition();
        if (gt != typeof(Nullable<>)) return nullIfNotNullable ? null : Type;
        var args = _typeInfo.GetGenericArguments();
        return args[0];
    }

    #region Properties

    public bool IsEnum                  => _typeInfo?.IsEnum ?? false;
    public bool IsValueType             => _typeInfo?.IsValueType ?? false;
    public bool IsGenericType           => _typeInfo?.IsGenericType ?? false;
    public bool IsGenericTypeDefinition => _typeInfo?.IsGenericTypeDefinition ?? false;

    public Type Type { get; }

    #endregion

    #region Fields

    private readonly Type _typeInfo;

    #endregion
}

/*
#if COREFX
    public partial struct ReflectionTypeWrapper
    {
        private readonly TypeInfo _typeInfo;
    }
#else
public partial struct ReflectionTypeWrapper
{
    private readonly Type _typeInfo;

}
#endif*/

