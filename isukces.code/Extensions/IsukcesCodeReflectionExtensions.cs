using System;
using System.Reflection;

namespace iSukces.Code;

public static class IsukcesCodeReflectionExtensions
{
    public static bool IsMemberStatic(this MemberInfo mi)
    {
        //todo: move to another type
        if (mi is MethodInfo methodInfo)
            return methodInfo.IsStatic;
        if (mi is PropertyInfo propertyInfo)
        {
            if (propertyInfo.CanRead)
            {
                var tmp = propertyInfo.GetGetMethod();
                if (tmp == null)
                    tmp = propertyInfo.GetSetMethod();
                if (tmp == null)
                {
                    var pi = mi.GetType()
#if COREFX
                            .GetTypeInfo()
#endif
                        .GetField("m_bindingFlags", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (pi != null)
                    {
                        var v = (BindingFlags)pi.GetValue(mi);
                        return v.HasFlag(BindingFlags.Static);
                    }
                }

                if (tmp == null)
                    throw new NotSupportedException();
                // ReSharper disable once TailRecursiveCall
                return IsMemberStatic(tmp);
            }
        }

        throw new NotSupportedException();
    }

    public static string? SimpleTypeName(this Type t)
    {
        if (t == typeof(string)) return "string";
        if (t == typeof(int)) return "int";
        if (t == typeof(uint)) return "uint";
        if (t == typeof(double)) return "double";
        if (t == typeof(float)) return "float";
        if (t == typeof(short)) return "short";
        if (t == typeof(ushort)) return "ushort";
        if (t == typeof(long)) return "long";
        if (t == typeof(ulong)) return "ulong";
        if (t == typeof(object)) return "object";
        if (t == typeof(bool)) return "bool";
        if (t == typeof(decimal)) return "decimal";
        if (t == typeof(byte)) return "byte";
        if (t == typeof(sbyte)) return "sbyte";

        {
            // nullable support 
            var nullableType = new ReflectionTypeWrapper(t).UnwrapNullable(true);
            if (nullableType != null)
            {
                var simple = SimpleTypeName(nullableType);
                if (string.IsNullOrEmpty(simple))
                    return null;
                return simple + "?";
            }
        }

        return null;
    }
}
