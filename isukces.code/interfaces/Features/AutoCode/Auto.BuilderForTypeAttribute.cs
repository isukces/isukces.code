using System;

namespace iSukces.Code.Interfaces;

public partial class Auto
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class BuilderForTypeAttribute : Attribute
    {
        public BuilderForTypeAttribute(Type targetType, params string[] skipWithFor)
        {
            TargetType  = targetType;
            SkipWithFor = skipWithFor ?? [];
        }

        public Type     TargetType     { get; }
        public string[] SkipWithFor    { get; } // sprawdzone not null
        public bool     SkipWithForAll { get; set; }
    }
        
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public sealed class BuilderForTypePropertyAttribute : Attribute
    {
        public BuilderForTypePropertyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; }

        public bool Create { get; set; }

        public Type Type { get; set; }

        public bool ExpandFlags { get; set; }
    }
}