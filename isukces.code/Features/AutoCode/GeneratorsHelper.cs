using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using isukces.code.interfaces;
using isukces.code.interfaces.Ammy;
using JetBrains.Annotations;

namespace isukces.code.AutoCode
{
    public static class GeneratorsHelper
    {
        public static void AddInitCode(CsClass cl, string codeLine)
        {
            var method = cl.Methods.FirstOrDefault(a => a.Name == AutoCodeInitMethodName);

            if (method is null)
                method = cl.AddMethod(AutoCodeInitMethodName, "void")
                    .WithVisibility(Visibilities.Private);

            method.Body += "\r\n" + codeLine;
        }

        public static CsExpression CallMethod(string instance, string method, params string[] arguments) =>
            (CsExpression)string.Format("{0}.{1}({2})", instance, method, string.Join(", ", arguments));

        public static CsExpression CallMethod(string method, params CsExpression[] arguments)
        {
            return (CsExpression)string.Format("{0}({1})", method, string.Join(", ", arguments.Select(a => a.Code)));
        }

        public static CsExpression CallMethod(string method, IArgumentsHolder holder) =>
            (CsExpression)string.Format("{0}({1})", method, string.Join(", ", holder.GetArguments()));

        public static CsExpression CallMethod(string instance, string method, IArgumentsHolder holder) =>
            (CsExpression)string.Format("{0}.{1}({2})", instance, method,
                string.Join(", ", holder.GetArguments()));

        public static MyStruct DefaultComparerMethodName(Type type, ITypeNameResolver resolver)
        {
            var comparer     = typeof(Comparer<>).MakeGenericType(type);
            var comparerName = resolver.TypeName(comparer);
            return new MyStruct("{0}.Compare", $"{comparerName}.Default");
        }

        public static string FieldName(string x) => "_" + x.Substring(0, 1).ToLower() + x.Substring(1);


        public static Type GetMemberResultType([NotNull] MemberInfo mi)
        {
            if (mi == null) throw new ArgumentNullException(nameof(mi));
            if (mi is PropertyInfo pi)
                return pi.PropertyType;
            if (mi is FieldInfo fi)
                return fi.FieldType;
            if (mi is MethodInfo mb)
                return mb.ReturnType;
            throw new NotSupportedException(mi.GetType().ToString());
        }

        public static string GetTypeName(this INamespaceContainer container, Type type)
        {
            var emitTypeAttribute = EmitTypeAttribute.GetAttribute(type);
            if (emitTypeAttribute != null)
            {
                var namespaceName = emitTypeAttribute.Namespace ?? type.Namespace;
                var shortName     = emitTypeAttribute.TypeName ?? type.Name;
                if (container?.IsKnownNamespace(namespaceName) ?? false)
                    return shortName;
                return namespaceName + "." + shortName;
            }

            //todo: Generic types
            if (type == null)
                return null;
            if (type.IsArray)
            {
                var arrayRank = type.GetArrayRank();
                var st        = GetTypeName(container, type.GetElementType());
                if (arrayRank < 2)
                    return st + "[]";
                return string.Format("{0}[{1}]", st, new string(',', arrayRank - 1));
            }

            var simple = type.SimpleTypeName();
            if (!string.IsNullOrEmpty(simple))
                return simple;
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (type.DeclaringType != null)
                return GetTypeName(container, type.DeclaringType) + "." + type.Name;

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
                                return GetTypeName(container, nullable) + "?";
                        }
                        var gt    = type.GetGenericTypeDefinition();
                        var args  = w.GetGenericArguments();
                        var args2 = string.Join(",", args.Select(a => GetTypeName(container, a)));
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

        public struct MyStruct
        {
            public MyStruct(string expressionTemplate, string instance = null)
            {
                Instance           = instance;
                ExpressionTemplate = expressionTemplate;
            }

            public string GetCode()
            {
                if (string.IsNullOrEmpty(Instance))
                    return ExpressionTemplate;
                return string.Format(ExpressionTemplate, Instance);
            }

            public string Instance           { get; }
            public string ExpressionTemplate { get; }
        }

        public const BindingFlags AllVisibility = BindingFlags.NonPublic | BindingFlags.Public;
        public const BindingFlags AllInstance = BindingFlags.Instance | AllVisibility;
        public const BindingFlags All = AllInstance | BindingFlags.Static;
        public const BindingFlags PublicInstance = BindingFlags.Public | BindingFlags.Instance;

        public const string StringEmpty = "string.Empty";
        public const string AutoCodeInitMethodName = "AutocodeInit";

        /*public static string TypeName<T>(this INamespaceContainer container)
        {
            return TypeName(container, typeof(T));
        }*/
    }
}