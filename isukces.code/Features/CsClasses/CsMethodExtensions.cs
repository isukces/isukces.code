using System;

namespace isukces.code
{
    public static class CsMethodExtensions
    {
        public static CsMethod WithAttribute(this CsMethod method, CsClass csClass, Type type)
        {
            return method.WithAttribute(csClass.TypeName(type));
        }

        public static CsMethod WithAttribute(this CsMethod method, string name)
        {
            name = CutAttributeSuffix(name);
            method.Attributes.Add(new CsAttribute(name));
            return method;
        }

        public static CsProperty WithAttribute(this CsProperty method, string name)
        {
            name = CutAttributeSuffix(name);
            method.Attributes.Add(new CsAttribute(name));
            return method;
        }

        public static CsMethod WithBody(this CsMethod method, string body)
        {
            method.Body = body;
            return method;
        }
        
        public static CsMethod WithOverride(this CsMethod method, bool isOverride=true)
        {
            method.IsOverride= isOverride;
            return method;
        }

        public static CsMethod WithBody(this CsMethod method, CodeFormatter code)
        {
            return WithBody(method, code?.Text);
        }

        public static CsMethod WithStatic(this CsMethod method, bool isStatic = true)
        {
            method.IsStatic = isStatic;
            return method;
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