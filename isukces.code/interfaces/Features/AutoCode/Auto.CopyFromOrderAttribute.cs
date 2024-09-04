#nullable enable
using System;

namespace iSukces.Code.Interfaces
{
    public partial class Auto
    {
        [AttributeUsage(AttributeTargets.Property)]
        public sealed class CopyFromOrderAttribute : Attribute
        {
            public CopyFromOrderAttribute(int order) => Order = order;

            public int Order { get; }
        }
    }
}