using System;
using System.Collections.Generic;
using JetBrains.Annotations;

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
            /// Name of property that denotes if object is empty.
            /// If true then no other properties will be compared.
            /// </summary>
            public string IsEmptyProperty { get; set; }

            [NotNull]
            public string[] GetHashCodeProperties { get; }

            /// <summary>
            ///     Użyj tych własności zamiast skanować
            /// </summary>
            public string UseOnlyPropertiesOrFields { get; set; }

            public GetHashCodeImplementationKind CachedGetHashCodeImplementation { get; set; }
        }
    }
    public enum GetHashCodeImplementationKind {
        /// <summary>
        /// Value is calculated in GetHashCode method
        /// </summary>
        Normal,
        /// <summary>
        /// Value is calculated on first GetHashCode call and stored in field 
        /// </summary>
        Cached,
        /// <summary>
        /// Value is calculated by calling method from constructor.
        /// Can be used for immutable classes. 
        /// </summary>
        Precomputed,
        
        /// <summary>
        /// No automatic code is generated
        /// </summary>
        Custom
    }
}