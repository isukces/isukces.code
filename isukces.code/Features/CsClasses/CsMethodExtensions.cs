using System;

namespace isukces.code
{
    public static class CsMethodExtensions
    {
        #region Static Methods

        public static CsMethod WithAttribute(this CsMethod method, CsClass csClass, Type type)
        {
            return method.WithAttribute(csClass.TypeName(type));
        }

        public static CsMethod WithAttribute(this CsMethod method, string name)
        {
            name = CutAttributeSuffix(name);
            method.Attributes.Add(new CsAttribute {Name = name});
            return method;
        }

        public static CsMethod WithBody(this CsMethod method, string body)
        {
            method.Body = body;
            return method;
        }

        public static CsMethod WithBody(this CsMethod method, CodeFormatter code)
        {
            return WithBody(method, code?.Text);
        }

        private static string CutAttributeSuffix(string name)
        {
            if (name.EndsWith(AttributeSuffix))
                name = name.Substring(0, name.Length - AttributeSuffixLength);
            return name;
        }

        #endregion

        #region Static Fields

        private static readonly int AttributeSuffixLength = AttributeSuffix.Length;

        #endregion

        #region Other

        private const string AttributeSuffix = "Attribute";

        #endregion
    }
}