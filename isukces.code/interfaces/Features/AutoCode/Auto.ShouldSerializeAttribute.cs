using System;

namespace isukces.code.interfaces
{
    public partial class Auto
    {
        [AttributeUsage(AttributeTargets.Property)]
        public class ShouldSerializeAttribute : Attribute
        {
            /// <summary>
            ///     Initializes a new instance of the <see cref="T:System.Attribute" /> class.
            /// </summary>
            public ShouldSerializeAttribute(string condition)
            {
                Condition = condition;
            }

            public ShouldSerializeAttribute()
            {
            }

            public string Condition { get; set; }
        }
        
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
        public class ShouldSerializeInfoAttribute : Attribute
        {
            public ShouldSerializeInfoAttribute(string codeTemplate)
            {
                CodeTemplate = codeTemplate ?? throw new ArgumentNullException(nameof(codeTemplate));
            }

            /// <summary>
            /// Code template, use {0} for value 
            /// </summary>
            public string CodeTemplate { get;   }
        }
    }
    
    
}