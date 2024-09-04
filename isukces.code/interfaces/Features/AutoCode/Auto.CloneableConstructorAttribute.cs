#nullable enable
using System;

namespace iSukces.Code.Interfaces
{
    public partial class Auto
    {
        [AttributeUsage(AttributeTargets.Constructor)]
        public class CloneableConstructorAttribute : Attribute
        {
            
        }
    }
}