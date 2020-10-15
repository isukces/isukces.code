using System;
using System.Reflection;
using iSukces.Code.Interfaces;

namespace iSukces.Code
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

        public static bool IsExplicityImplementation(this Type implementingType, Type interfaceType, string methodName)
        {
            var map = implementingType.GetInterfaceMap(interfaceType);
            for (var index = map.InterfaceMethods.Length - 1; index >= 0; index--)
            {
                var interfaceMethod = map.InterfaceMethods[index];
                if (interfaceMethod.Name != methodName) continue;
                var targetMethod = map.TargetMethods[index];
                return targetMethod.Name.Contains(".");
            }

            return true;
        }

        public static bool IsExplicityImplementation<TInterface>(this Type implementingType, string methodName)
        {
            return IsExplicityImplementation(implementingType, typeof(TInterface), methodName);
        }

        public static bool IsNullableType(this Type type)
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

        public static Type StripNullable(this Type type, out bool wasNullable)
        {
#if COREFX
            var typeInfo = type.GetTypeInfo();
#else
            var typeInfo = type;
#endif
            if (typeInfo.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                wasNullable = true;
                return typeInfo.GetGenericArguments()[0];
            }

            wasNullable = false;
            return type;
        }
    }
}