using System;
using iSukces.Code.AutoCode;

namespace iSukces.Code.Interfaces
{
    public partial class Auto
    {
        /// <summary>
        /// Used by <see cref="CopyFromGenerator">CopyFromGenerator</see>
        /// </summary>
        [AttributeUsage(AttributeTargets.Class)]
        public class CloneableAttribute : Attribute
        {
        }
    }
}