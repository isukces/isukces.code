using System;
using System.Diagnostics;

namespace iSukces.Code.Interfaces;

public static partial class Auto
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
    [Conditional("AUTOCODE_ANNOTATIONS")]
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