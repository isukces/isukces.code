using System;

namespace iSukces.Code.Interfaces;

public static partial class Auto
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ReactivePropertyAttribute : PropertyAttributeBase
    {
        public ReactivePropertyAttribute(string name, Type propertyType, string? description = null)
        {
            Name         = name;
            PropertyType = propertyType;
            Description  = description;
        }

        public Visibilities FieldVisibility { get; set; } = Visibilities.Private;
    }
}