using System;

namespace iSukces.Code.Interfaces
{
    public partial class Auto
    {
        [AttributeUsage(AttributeTargets.Class)]
        public class Cloneable : Attribute
        {
        }
    }
}