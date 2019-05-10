using System;
using System.Reflection;
using isukces.code.interfaces;

namespace isukces.code
{
    public static class TypeExtensions
    {
        public static CsNamespaceMemberKind GetNamespaceMemberKind(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var ti = type.GetTypeInfo();
            if (ti.IsInterface)
                return CsNamespaceMemberKind.Interface;
            if (ti.IsValueType)
                return CsNamespaceMemberKind.Struct;
            return CsNamespaceMemberKind.Class;
        }

        public static bool IsNullable(this Type type)
        {
#if COREFX
            var typeInfo = type.GetTypeInfo();
#else
            var typeInfo = type;
#endif
            return typeInfo.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static Type StripNullable(this Type type)
        {
#if COREFX
            var typeInfo = type.GetTypeInfo();
#else
            var typeInfo = type;
#endif
            if (typeInfo.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return typeInfo.GetGenericArguments()[0];
            return type;
        }
    }
}