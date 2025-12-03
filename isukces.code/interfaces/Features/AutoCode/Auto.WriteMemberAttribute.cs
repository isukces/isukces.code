using System;
using System.Diagnostics;

namespace iSukces.Code.Interfaces;

public static partial class Auto
{
    [AttributeUsage(AttributeTargets.Property)]
    [Conditional("AUTOCODE_ANNOTATIONS")]
    public class WriteMemberAttribute : Attribute
    {
        public WriteMemberAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}