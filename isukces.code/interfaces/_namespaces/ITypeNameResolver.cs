using System;
using System.Collections.Generic;
using System.Linq;
using iSukces.Code.AutoCode;

namespace iSukces.Code.Interfaces;

public interface ITypeNameResolver
{
    CsType GetTypeName(Type type);
}

public static class TypeToNameResolverExtensions
{
    extension(ITypeNameResolver s)
    {
        public string GetTypeNameD(Type type)
        {
            return s.GetTypeName(type).Declaration;
        }

        public string GetTypeNameD<T>()
        {
            return s.GetTypeName(typeof(T)).Declaration;
        }

        public string GetEnumFlagsValueCode<T>(T value,
            Func<IReadOnlyList<string>, string>? joinFunc = null)
            where T : Enum
        {
            var c = s.GetTypeName<T>().Declaration;
            var e = CsEnumHelper.Get(typeof(T));

            var    names = e.GetFlagStrings(value, c);
            string result;
            if (joinFunc is not null)
                result = joinFunc(names.ToArray());
            else
                result = string.Join(" | ", names);

            return result;
        }

        public string GetEnumValueCode<T>(T value)
            where T : Enum
        {
            var c      = s.GetTypeName<T>();
            var value2 = c.GetMemberCode(value.ToString());
            return value2;
        }

        public string GetMemeberName(Type type, string instanceName) =>
            s.GetTypeName(type).GetMemberCode(instanceName);

        public string GetMemeberName<T>(string instanceName) =>
            s.GetTypeName<T>().GetMemberCode(instanceName);

        public CsType GetTypeName<T>()
            => s.GetTypeName(typeof(T));
    }

   
}
