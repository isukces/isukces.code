using System;

namespace isukces.code.interfaces
{
    public interface ITypeToNameResolver
    {
        string TypeName(Type type);
    }

    public static class TypeToNameResolverExtensions
    {
        public static string GetMemeberName(this ITypeToNameResolver resolver, Type type, string instanceName)
        {
            return resolver.TypeName(type) + "." + instanceName;
        }

        public static string GetMemeberName<T>(this ITypeToNameResolver resolver, string instanceName)
        {
            return resolver.TypeName<T>() + "." + instanceName;
        }

        public static string TypeName<T>(this ITypeToNameResolver resolver)
        {
            return resolver.TypeName(typeof(T));
        }
    }
}