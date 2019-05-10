using System;

namespace isukces.code.interfaces
{
    public partial class Auto
    {
        /// <summary>
        /// Treat null as string.Empty or collection without elements 
        /// </summary>
        public class NullIsEmptyAttribute : Attribute
        {
        
        }
        
        
        /// <summary>
        /// when both a.X and b.X are null then a.X is equal to b.X
        /// </summary>
        public class NullAreEqualAttribute : Attribute
        {
        
        }
    }
}