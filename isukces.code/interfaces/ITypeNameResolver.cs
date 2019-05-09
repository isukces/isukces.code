using System;

namespace isukces.code.interfaces
{
    public interface ITypeNameResolver
    {
        string GetTypeName(Type type);
    }

    public static class TypeToNameResolverExtensions
    {
        public static string GetMemeberName(this ITypeNameResolver resolver, Type type, string instanceName)
        {
            return resolver.GetTypeName(type) + "." + instanceName;
        }

        public static string GetMemeberName<T>(this ITypeNameResolver resolver, string instanceName)
        {
            return resolver.GetTypeName<T>() + "." + instanceName;
        }

        public static string GetTypeName<T>(this ITypeNameResolver resolver)
        {
            return resolver.GetTypeName(typeof(T));
        }

        [Obsolete("Use GetTypeName")]
        public static string TypeName(this ITypeNameResolver resolver, Type type)
        {
            return resolver.GetTypeName(type);
        }
    }
}