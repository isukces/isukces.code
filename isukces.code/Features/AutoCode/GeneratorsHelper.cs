using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using isukces.code.interfaces;

namespace isukces.code.AutoCode
{
    public static class GeneratorsHelper
    {
        public static string DefaultComparerMethodName(Type type, ITypeToNameResolver resolver)
        {
            var comparer     = typeof(Comparer<>).MakeGenericType(type);
            var comparerName = resolver.TypeName(comparer);
            return $"{comparerName}.Default.Compare";
        }

        public static string FieldName(string x)
        {
            return "_" + x.Substring(0, 1).ToLower() + x.Substring(1);
        }

        public static string GetWriteMemeberName(PropertyInfo pi)
        {
            var props = pi.GetCustomAttribute<Auto.WriteMemberAttribute>();
            return !string.IsNullOrEmpty(props?.Name) ? props.Name : pi.Name;
        }

        public static HashSet<T> MakeCopy<T>(IEnumerable<T> source, IEnumerable<T> append = null,
            IEnumerable<T> remove = null)
        {
            var s = new HashSet<T>();
            if (source != null)
                foreach (var i in source)
                    s.Add(i);
            if (append != null)
                foreach (var i in append)
                    s.Add(i);
            if (remove != null)
                foreach (var i in remove)
                    s.Remove(i);
            return s;
        }

        public static string TypeName<T>(this INamespaceContainer container)
        {
            return TypeName(container, typeof(T));
        }

        public static string TypeName(this INamespaceContainer container, Type type)
        {
            //todo: Generic types
            if (type == null)
                return null;
            if (type.IsArray)
            {
                var arrayRank = type.GetArrayRank();
                var st        = TypeName(container, type.GetElementType());
                if (arrayRank < 2)
                    return st + "[]";
                return string.Format("{0}[{1}]", st, new string(',', arrayRank - 1));
            }

            var simple = type.SimpleTypeName();
            if (!string.IsNullOrEmpty(simple))
                return simple;
            if (type.DeclaringType != null)
                return TypeName(container, type.DeclaringType) + "." + type.Name;

            string fullName;
            {
                var w = new ReflectionTypeWrapper(type);
                if (w.IsGenericType)
                {
                    if (w.IsGenericTypeDefinition)
                    {
                        var args  = w.GetGenericArguments();
                        var args2 = string.Join(",", args.Select(a => a.Name));
                        fullName = type.FullName.Split('`')[0] + "<" + args2 + ">";
                    }
                    else
                    {
                        {
                            var nullable = w.UnwrapNullable(true);
                            if (nullable != null)
                                return TypeName(container, nullable) + "?";
                        }
                        var gt    = type.GetGenericTypeDefinition();
                        var args  = w.GetGenericArguments();
                        var args2 = string.Join(",", args.Select(a => TypeName(container, a)));
                        fullName = gt.FullName.Split('`')[0] + "<" + args2 + ">";
                    }
                }
                else
                {
                    fullName = type.FullName;
                }
            }
            var typeNamespace = type.Namespace;
            var canCut        = container?.IsKnownNamespace(typeNamespace) ?? false;
            return canCut
                ? fullName.Substring(typeNamespace.Length + 1)
                : fullName;
        }

        public const BindingFlags AllVisibility = BindingFlags.NonPublic | BindingFlags.Public;
        public const BindingFlags AllInstance = BindingFlags.Instance | AllVisibility;
        public const BindingFlags All = AllInstance | BindingFlags.Static;
        public const BindingFlags PublicInstance = BindingFlags.Public | BindingFlags.Instance;

        public const string StringEmpty = "string.Empty";
    }
}