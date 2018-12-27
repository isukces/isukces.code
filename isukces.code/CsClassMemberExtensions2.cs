using System;
using isukces.code.interfaces;

namespace isukces.code
{
    public static class CsClassMemberExtensions2
    {
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