using System;
using System.Reflection;
using isukces.code.interfaces;

namespace isukces.code
{
    public static class CsClassMemberExtensions2
    {
        public static NamespaceMemberKind GetNamespaceMemberKind(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var ti = type.GetTypeInfo();
            if (ti.IsInterface)
                return NamespaceMemberKind.Interface;
            if (ti.IsValueType)
                return NamespaceMemberKind.Struct;
            return NamespaceMemberKind.Class;
        }

        public static T WithAttribute<T>(this T method, string name)
            where T : ICsClassMember
        {
            name = CutAttributeSuffix(name);
            method.Attributes.Add(new CsAttribute(name));
            return method;
        }

        public static T WithAttribute<T>(this T method, CsClass csClass, Type type)
            where T : ICsClassMember
        {
            return method.WithAttribute(csClass.TypeName(type));
        }

        private static string CutAttributeSuffix(string name)
        {
            if (name.EndsWith(AttributeSuffix))
                name = name.Substring(0, name.Length - AttributeSuffixLength);
            return name;
        }

        private static readonly int AttributeSuffixLength = AttributeSuffix.Length;

        private const string AttributeSuffix = "Attribute";
    }
}