using System;
using System.Diagnostics;

namespace iSukces.Code.Interfaces;

public static partial class Auto
{
    [AttributeUsage(AttributeTargets.Property)]
    [Conditional("AUTOCODE_ANNOTATIONS")]
    public sealed class CopyFromOrderAttribute : Attribute
    {
        public CopyFromOrderAttribute(int order) => Order = order;

        public int Order { get; }
    }
}