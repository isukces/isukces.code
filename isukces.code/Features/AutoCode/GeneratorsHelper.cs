using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using isukces.code.interfaces;

namespace isukces.code.AutoCode
{
    internal class GeneratorsHelper
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

        public static bool IsMemberStatic(MemberInfo mi)
        {
            //todo: move to another type
            var methodInfo = mi as MethodInfo;
            if (methodInfo != null)
                return methodInfo.IsStatic;
            var propertyInfo = mi as PropertyInfo;
            if (propertyInfo != null)
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
                var st = TypeName(type.GetElementType(), container);
                if (arrayRank < 2)
                    return st + "[]";
                return string.Format("{0}[{1}]", st, new string(',', arrayRank - 1));
            }
            var simple = SimpleTypeName(type);
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
                        var nullable = GetNullableType(type);
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

        private static Type GetNullableType(Type type)
        {
            if ((type == null) || !type
#if COREFX
                    .GetTypeInfo()
#endif
                    
                    .IsGenericType)
                return null;
            var gt = type.GetGenericTypeDefinition();
            var args = type
#if COREFX
                .GetTypeInfo()
#endif

                .GetGenericArguments();
            return gt == typeof(Nullable<>) ? args[0] : null;
        }

        private static string SimpleTypeName(Type t)
        {
            if (t == typeof(string)) return "string";
            if (t == typeof(int)) return "int";
            if (t == typeof(uint)) return "uint";
            if (t == typeof(double)) return "double";
            if (t == typeof(short)) return "short";
            if (t == typeof(ushort)) return "ushort";
            if (t == typeof(object)) return "object";
            if (t == typeof(bool)) return "bool";
            if (t == typeof(decimal)) return "decimal";
            if (t == typeof(byte)) return "byte";

            {
                // nullable support 
                var nullableType = GetNullableType(t);
                if (nullableType != null)
                {
                    var simple = SimpleTypeName(nullableType);
                    if (string.IsNullOrEmpty(simple))
                        return null;
                    return simple + "?";
                }
            }

            return null;

            // var ns = t.Namespace;
            // var fullName = t.FullName.Split('`')[0];

            // System.Nullable`1[[System.Int16, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]

            /*
            foreach (var i in CopyMeMaker.GetNameSpaces().OrderByDescending(a => a.Length))
            {
                if (ns != i) continue;
                var c = fullName.Substring(ns.Length + 1);
                if (!c.Contains("."))
                    fullName = c;
                else
                    fullName = c;
                break;
            }
            if (args != null)
            {
                fullName += "<" + String.Join(",", args.Select(TypeName)) + ">";
            }
            if (fullName.Contains("+"))
                fullName = fullName.Replace("+", ".");
            return fullName;
            */
        }

        public const BindingFlags All =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
    }
}