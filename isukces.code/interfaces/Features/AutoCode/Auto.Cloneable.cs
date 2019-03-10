using System;

namespace isukces.code.interfaces
{
    public partial class Auto
    {
        [AttributeUsage(AttributeTargets.Class)]
        public class Cloneable : Attribute
        {
        }
    }
}