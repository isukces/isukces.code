using System;

namespace iSukces.Code.Interfaces
{
    public partial class Auto
    {
        [AttributeUsage(AttributeTargets.Property)]
        public sealed class CopyFromOrderAttribute : Attribute
        {
            public int Order { get; }
        }
    }
}