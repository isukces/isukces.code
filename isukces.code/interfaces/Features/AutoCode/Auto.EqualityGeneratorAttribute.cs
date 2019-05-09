using System;
using System.Collections.Generic;

namespace isukces.code.interfaces
{
    public partial class Auto
    {
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
        public class EqualityGeneratorAttribute : Attribute
        {
            public EqualityGeneratorAttribute()
            {
                SkipProperties = new HashSet<string>();
            }

            public EqualityGeneratorAttribute(string isEmptyProperty, params string[] skipProperties)
            {
                IsEmptyProperty = isEmptyProperty;
                SkipProperties  = skipProperties.ToHashSet();
            }

            public HashSet<string> SkipProperties { get; }

            /// <summary>
            /// Name of property that denotes if object is empty. If true then no other properties will be compared.
            /// </summary>
            public string IsEmptyProperty { get; set; }

            public string GetHashCodeProperties { get; set; }

            /// <summary>
            ///     Użyj tych własności zamiast skanować
            /// </summary>
            public string UseOnlyPropertiesOrFields { get; set; }

            public bool CachedGetHashCodeImplementation { get; set; }
        }
    }
}