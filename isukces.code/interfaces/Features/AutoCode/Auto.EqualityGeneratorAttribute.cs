using System;

namespace iSukces.Code.Interfaces
{
    public partial class Auto
    {
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
        public class EqualityGeneratorAttribute : Attribute
        {
            public EqualityGeneratorAttribute(string? isEmptyProperty=null)
            {
                IsEmptyProperty = isEmptyProperty;
            }

            /// <summary>
            /// Name of property that denotes if object is empty.
            /// If true then no other properties will be compared.
            /// </summary>
            public string IsEmptyProperty { get; set; }

            public string[] GetHashCodeProperties { get; set; }

            /// <summary>
            ///    Use this properties or fields instead of all properties
            /// </summary>
            public string UseOnlyPropertiesOrFields { get; set; }

            public GetHashCodeImplementationKind CachedGetHashCodeImplementation { get; set; }

            public bool UseGetHashCodeInEqualityChecking { get; set; }
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
