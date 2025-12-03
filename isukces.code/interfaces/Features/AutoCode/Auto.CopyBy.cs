using System;
using System.Diagnostics;

namespace iSukces.Code.Interfaces;

public static partial class Auto
{
    public class CopyBy
    {
        [AttributeUsage(AttributeTargets.Property)]
        [Conditional("AUTOCODE_ANNOTATIONS")]
        public class ReferenceAttribute : Attribute
        {
        }

        [AttributeUsage(AttributeTargets.Property)]
        [Conditional("AUTOCODE_ANNOTATIONS")]
        public class ValuesProcessorAttribute : Attribute
        {
        }

        [AttributeUsage(AttributeTargets.Property)]
        [Conditional("AUTOCODE_ANNOTATIONS")]
        // ReSharper disable once MemberHidesStaticFromOuterClass
        public class CloneableAttribute : Attribute
        {
        }
    }
}