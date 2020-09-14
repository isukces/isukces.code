using System;

namespace iSukces.Code
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = true)]
    public class DependsOnPropertyAttribute:Attribute
    {
        public DependsOnPropertyAttribute(params string[] propertyNames)
        {
            PropertyNames = propertyNames;
        }

        public string[] PropertyNames { get; set; }
    }
}