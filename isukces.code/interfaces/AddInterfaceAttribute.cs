using System;

namespace isukces.code.interfaces
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
