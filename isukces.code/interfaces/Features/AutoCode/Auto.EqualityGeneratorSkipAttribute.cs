using System;

// ReSharper disable once CheckNamespace
namespace iSukces.Code.Interfaces
{
    public partial class Auto
    {
        /// <summary>
        /// Property or field decorated with this attribute will not be included
        /// in Equals nor GetHashCode method 
        /// </summary>
        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
        public class EqualityGeneratorSkipAttribute : Attribute
        {
            
        }
    }
}
