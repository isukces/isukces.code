using System;
using System.Linq;
using iSukces.Code.AutoCode;

namespace iSukces.Code.Interfaces;

public static class NamespaceContainerExtensions
{
    extension(INamespaceContainer self)
    {
        public CsType GetTypeName(string namespaceName, string shortName)
        {
            var info = self.GetNamespaceInfo(namespaceName);
            return info.SearchResult != NamespaceSearchResult.NotFound
                ? info.AddAlias(shortName)
                : new CsType($"{namespaceName}.{shortName}");
        }

        public CsType GetTypeName(Type? type)
        {
            //todo: Generic types
            if (type is null)
                return CsType.Void;
            if (type.IsArray)
            {
                var arrayRank = type.GetArrayRank();
                var st        = GetTypeName(self, type.GetElementType());
                return st.MakeArray(arrayRank);
            }

            var simple = type.SimpleTypeName();
            if (!string.IsNullOrEmpty(simple))
                return new CsType(simple);
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (type.DeclaringType is not null)
                return GetTypeName(self, type.DeclaringType).AppendBase("." + type.Name);
            {
                var alias = self?.TryGetTypeAlias(TypeProvider.FromType(type));
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
                                var n1 = GetTypeName(self, nullable);
                                n1.Nullable = NullableKind.ValueNullable;
                                return n1;
                            }
                        }
                        var gt = type.GetGenericTypeDefinition();
                        mainPart = gt.FullName!.Split('`')[0];
                        var args = w.GetGenericArguments();

                        generics = args.Select(a => GetTypeName(self, a))
                            .ToArray();
                    }
                }
                else
                    mainPart = type.FullName!;
            }
            var typeNamespace = type.Namespace ?? "";

            var    nsInfo = self?.GetNamespaceInfo(typeNamespace) ?? CsType.MakeDefault(typeNamespace);
            CsType result;
            switch (nsInfo.SearchResult)
            {
                case NamespaceSearchResult.Empty: result    = new CsType(mainPart); break;
                case NamespaceSearchResult.Found: result    = nsInfo.AddAlias(mainPart.Substring(typeNamespace.Length + 1)); break;
                case NamespaceSearchResult.NotFound: result = new CsType(mainPart); break;
                default: throw new ArgumentOutOfRangeException();
            }

            result.GenericParamaters = generics;
            return result;
        }

        [Obsolete("Use GetNamespaceInfo().IsKnownWithoutAlias()")]
        public bool IsKnownNamespace(string? namespaceName)
        {
            if (string.IsNullOrEmpty(namespaceName))
                return true;
            var a = self.GetNamespaceInfo(namespaceName);
            return a.IsKnownWithoutAlias();
        }

        public CsType Reduce(CsType type)
        {
            var (ns, shortTypeName) = type.SpitNamespaceAndShortName();

            return self.GetTypeName(ns, shortTypeName);
        }
    }
}
