using System;

namespace iSukces.Code.Interfaces
{
    public partial class Auto
    {
        public class CopyBy
        {
            [AttributeUsage(AttributeTargets.Property)]
            public class ReferenceAttribute : Attribute
            {
            }

            [AttributeUsage(AttributeTargets.Property)]
            public class ValuesProcessorAttribute : Attribute
            {
            }

            [AttributeUsage(AttributeTargets.Property)]
            // ReSharper disable once MemberHidesStaticFromOuterClass
            public class CloneableAttribute : Attribute
            {
            }
        }
    }
}