#nullable enable
using System;

namespace iSukces.Code.Interfaces
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
    public class AddInterfaceAttribute : Attribute
    {
        #region Constructors
        public AddInterfaceAttribute(Type _interface)
        {
            Interface = _interface;
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// interfejs implementowany przez klasę
        /// </summary>
        public Type Interface { get; set; }

        #endregion

    }
}
