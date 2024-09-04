#nullable enable
using System;

namespace iSukces.Code
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DependsOnPropertyAttribute : Attribute
    {
        public DependsOnPropertyAttribute(params string[] propertyNames) { PropertyNames = propertyNames; }


        public string[]               PropertyNames { get; set; }
        public DependsOnPropertyFlags Flags         { get; set; }
    }

    [Flags]
    public enum DependsOnPropertyFlags
    {
        None,
        SkipCreatingConstants,
        /// <summary>
        /// When <see cref="DependsOnPropertyGenerator">Exclude property in GetDependentProperties method</see>
        /// <see cref="DependsOnPropertyGeneratorFlags">DependsOnPropertyGeneratorF</see>.
        /// <see cref="DependsOnPropertyGeneratorFlags.GetDependentProperties">GetDependentProperties</see>
        /// </summary>
        ExcludeFromGetDependentPropertiesMethod
    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class DependsOnProperty2 : Attribute
    {
        

        public bool SkipCreatingConstants { get; set; }
        
    }
}