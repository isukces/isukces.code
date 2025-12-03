using System;
using System.Diagnostics;

namespace iSukces.Code.Interfaces;

public static partial class Auto
{
    [AttributeUsage(AttributeTargets.Property)]
    [Conditional("AUTOCODE_ANNOTATIONS")]
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
}