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

        public static Type StripNullable(this Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return typeInfo.GetGenericArguments()[0];
            return type;
        }
    }
}