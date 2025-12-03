using System;
using System.Reflection;
using iSukces.Code.Interfaces;

namespace iSukces.Code;

public static class TypeExtensions
{
    extension(Type type)
    {
        public CsNamespaceMemberKind GetNamespaceMemberKind()
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            var ti = type.GetTypeInfo();
            if (ti.IsInterface)
                return CsNamespaceMemberKind.Interface;
            if (ti.IsValueType)
                return CsNamespaceMemberKind.Struct;
            return CsNamespaceMemberKind.Class;
        }

        public bool IsExplicityImplementation(Type interfaceType, string methodName)
        {
            var map = type.GetInterfaceMap(interfaceType);
            for (var index = map.InterfaceMethods.Length - 1; index >= 0; index--)
            {
                var interfaceMethod = map.InterfaceMethods[index];
                if (interfaceMethod.Name != methodName) continue;
                var targetMethod = map.TargetMethods[index];
                return targetMethod.Name.Contains('.');
            }

            return true;
        }

        public bool IsExplicityImplementation<TInterface>(string methodName)
        {
            return IsExplicityImplementation(type, typeof(TInterface), methodName);
        }


        public bool IsNullableType()
        {
#if COREFX
            var typeInfo = type.GetTypeInfo();
#else
            var typeInfo = type;
#endif
            return typeInfo.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public Type StripNullable()
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

        public Type StripNullable(out bool wasNullable)
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