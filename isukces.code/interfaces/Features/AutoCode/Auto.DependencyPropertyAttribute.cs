using System;
using System.Diagnostics;

namespace iSukces.Code.Interfaces;

public static partial class Auto
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    [Conditional("AUTOCODE_ANNOTATIONS")]
    public class DependencyPropertyAttribute : Attribute
    {
        public DependencyPropertyAttribute(string name, Type propertyType)
        {
            Name         = name;
            PropertyType = propertyType;
        }

        public string Name         { get; set; }
        public Type   PropertyType { get; set; }

        public string PropertyChanged { get; set; }

        public object DefaultValue { get; set; }

        public string Coerce { get; set; }
    }
}