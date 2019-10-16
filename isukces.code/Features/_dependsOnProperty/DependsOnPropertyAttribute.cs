using System;

namespace isukces.code._dependsOnProperty
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