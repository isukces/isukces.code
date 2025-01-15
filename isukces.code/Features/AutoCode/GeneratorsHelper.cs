using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using iSukces.Code.Interfaces;

namespace iSukces.Code.AutoCode
{
    public static class GeneratorsHelper
    {
        public static void AddInitCode(CsClass cl, string codeLine)
        {
            var method = cl.Methods.FirstOrDefault(a => a.Name == AutoCodeInitMethodName);

            if (method is null)
                method = cl.AddMethod(AutoCodeInitMethodName, CsType.Void)
                    .WithVisibility(Visibilities.Private);

            method.Body += "\r\n" + codeLine;
        }

        public static CsExpression CallMethod(string instance, string method, params string[] arguments)
        {
            var prefix = $"{instance}.{method}";
            var code   = arguments.CommaJoin().Parentheses(prefix);
            return (CsExpression)code;
        }

        public static CsExpression CallMethod(string method, params CsExpression[] arguments)
        {
            var code = arguments.Select(a => a.Code)
                .CommaJoin()
                .Parentheses(method);
            return (CsExpression)code;
        }

        public static CsExpression CallMethod(string method, IArgumentsHolder holder)
        {
            var code   = holder.GetArguments().CommaJoin().Parentheses(method);
            return (CsExpression)code;
        }

        public static CsExpression CallMethod(string instance, string method, IArgumentsHolder holder)
        {
            var prefix = instance + "." + method;
            var code   = holder.GetArguments().CommaJoin().Parentheses(prefix);
            return (CsExpression)code;
        }

        public static MyStruct DefaultComparerMethodName(Type type, ITypeNameResolver resolver)
        {
            var comparer     = typeof(Comparer<>).MakeGenericType(type);
            var comparerName = resolver.GetTypeName(comparer).Declaration;
            return new MyStruct("{0}.Compare", $"{comparerName}.Default");
        }

        public static string FieldName(string x) => "_" + x.Substring(0, 1).ToLower() + x.Substring(1);


        public static Type GetMemberResultType(MemberInfo mi)
        {
            if (mi is null) throw new ArgumentNullException(nameof(mi));
            if (mi is PropertyInfo pi)
                return pi.PropertyType;
            if (mi is FieldInfo fi)
                return fi.FieldType;
            if (mi is MethodInfo mb)
                return mb.ReturnType;
            throw new NotSupportedException(mi.GetType().ToString());
        }

        public static CsType GetTypeName(this INamespaceContainer? container, Type? type)
        {
            //todo: Generic types
            if (type is null)
                return CsType.Void;
            if (type.IsArray)
            {
                var arrayRank = type.GetArrayRank();
                var st        = GetTypeName(container, type.GetElementType());
                return st.MakeArray(arrayRank);
            }

            var simple = type.SimpleTypeName();
            if (!string.IsNullOrEmpty(simple))
                return new CsType(simple);
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (type.DeclaringType is not null)
                return GetTypeName(container, type.DeclaringType).AppendBase("." + type.Name);
            {
                var alias = container?.TryGetTypeAlias(TypeProvider.FromType(type));
                if (!string.IsNullOrEmpty(alias))
                    return new CsType(alias);
            }
            var    generics = Array.Empty<CsType>();
            string mainPart;
            {
                var w = new ReflectionTypeWrapper(type);
                if (w.IsGenericType)
                {
                    if (w.IsGenericTypeDefinition)
                    {
                        var args = w.GetGenericArguments();
                        generics = args.Select(a => (CsType)a.Name).ToArray();
                        mainPart = type.FullName!.Split('`')[0];
                    }
                    else
                    {
                        {
                            var nullable = w.UnwrapNullable(true);
                            if (nullable is not null)
                            {
                                var n1 = GetTypeName(container, nullable);
                                n1.Nullable = NullableKind.ValueNullable;
                                return n1;
                            }
                        }
                        var gt = type.GetGenericTypeDefinition();
                        mainPart = gt.FullName!.Split('`')[0];
                        var args = w.GetGenericArguments();

                        generics = args.Select(a => GetTypeName(container, a))
                            .ToArray();
                    }
                }
                else
                    mainPart = type.FullName!;
            }
            var typeNamespace = type.Namespace ?? "";

            var    nsInfo = container?.GetNamespaceInfo(typeNamespace) ?? CsType.MakeDefault(typeNamespace);
            CsType result;
            switch (nsInfo.SearchResult)
            {
                case NamespaceSearchResult.Empty: result  = new CsType(mainPart); break;
                case NamespaceSearchResult.Found: result  = nsInfo.AddAlias(mainPart.Substring(typeNamespace.Length + 1)); break;
                case NamespaceSearchResult.NotFound: result = new CsType(mainPart); break;
                default: throw new ArgumentOutOfRangeException();
            }

            result.GenericParamaters = generics;
            return result;
        }

        public static string GetWriteMemeberName(PropertyInfo pi)
        {
            var props = pi.GetCustomAttribute<Auto.WriteMemberAttribute>();
            return !string.IsNullOrEmpty(props?.Name) ? props.Name : pi.Name;
        }

        public static HashSet<T> MakeCopy<T>(IEnumerable<T>? source, IEnumerable<T>? append = null,
            IEnumerable<T>? remove = null)
        {
            var s = new HashSet<T>();
            if (source is not null)
                foreach (var i in source)
                    s.Add(i);
            if (append is not null)
                foreach (var i in append)
                    s.Add(i);
            if (remove is not null)
                foreach (var i in remove)
                    s.Remove(i);
            return s;
        }

        public struct MyStruct
        {
            public MyStruct(string expressionTemplate, string? instance = null)
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
        public const BindingFlags AllStatic = BindingFlags.Static | AllVisibility;
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
