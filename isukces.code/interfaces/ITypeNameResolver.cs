using System;
using System.Collections.Generic;
using System.Linq;

namespace iSukces.Code.Interfaces;

public interface ITypeNameResolver
{
    CsType GetTypeName(Type type);
}

public static class TypeToNameResolverExtensions
{
    public static string GetEnumFlagsValueCode<T>(this ITypeNameResolver resolver, T value,
        Func<IReadOnlyList<string>, string> joinFunc = null)
        where T : Enum
    {
        var c = resolver.GetTypeName<T>().Declaration;
        var e = CsEnumHelper.Get(typeof(T));

        var    names = e.GetFlagStrings(value, c);
        string result;
        if (joinFunc != null)
            result = joinFunc(names.ToArray());
        else
            result = string.Join(" | ", names);

        return result;
    }

    public static string GetEnumValueCode<T>(this ITypeNameResolver resolver, T value)
        where T : Enum
    {
        var c      = resolver.GetTypeName<T>();
        var value2 = c.GetMemberCode(value.ToString());
        return value2;
    }

    public static string GetMemeberName(this ITypeNameResolver resolver, Type type, string instanceName) =>
        resolver.GetTypeName(type).GetMemberCode(instanceName);

    public static string GetMemeberName<T>(this ITypeNameResolver resolver, string instanceName) =>
        resolver.GetTypeName<T>().GetMemberCode(instanceName);

    public static CsType GetTypeName<T>(this ITypeNameResolver resolver) => resolver.GetTypeName(typeof(T));

    /*
    [Obsolete("Use GetTypeName")]
    public static string TypeName(this ITypeNameResolver resolver, Type type) => resolver.GetTypeName(type);
*/
}