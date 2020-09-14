using System;

namespace iSukces.Code.Interfaces
{
    public partial class Auto
    {
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
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