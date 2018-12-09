using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using isukces.code.interfaces;

namespace isukces.code.AutoCode
{
    public class GeneratorsHelper
    {
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

        public static string TypeName(Type type, INamespaceContainer container)
        {
            //todo: Generic types
            if (type == null)
                return null;
            if (type.IsArray)
            {
                var arrayRank = type.GetArrayRank();
                var st        = TypeName(type.GetElementType(), container);
                if (arrayRank < 2)
                    return st + "[]";
                return string.Format("{0}[{1}]", st, new string(',', arrayRank - 1));
            }

            var simple = IsukcesCodeReflectionExtensions.SimpleTypeName(type);
            if (!string.IsNullOrEmpty(simple))
                return simple;
            if (type.DeclaringType != null)
                return TypeName(type.DeclaringType, container) + "." + type.Name;

            string fullName;
            {
                if (type
#if COREFX
                    .GetTypeInfo()
#endif
                    .IsGenericType)
                {
                    {
                        var nullable = IsukcesCodeReflectionExtensions.GetNullableType(type);
                        if (nullable != null)
                            return TypeName(nullable, container) + "?";
                    }
                    var gt = type.GetGenericTypeDefinition();
                    var args = type
#if COREFX
                        .GetTypeInfo()
#endif
                        .GetGenericArguments();
                    var args2 = string.Join(",", args.Select(a => TypeName(a, container)));
                    fullName = gt.FullName.Split('`')[0] + "<" + args2 + ">";
                }
                else
                    fullName = type.FullName;
            }
            var typeNamespace = type.Namespace;
            return !string.IsNullOrEmpty(typeNamespace) &&
                   (container?.GetNamespaces(true)?.Contains(typeNamespace) ?? false)
                ? fullName.Substring(typeNamespace.Length + 1)
                : fullName;
        }

        public const BindingFlags All =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
    }
}