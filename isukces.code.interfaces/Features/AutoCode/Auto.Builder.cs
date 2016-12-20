#region using

using System;

#endregion

namespace isukces.code.interfaces
{
    public partial class Auto
    {
        #region Nested

        public class BuilderAttribute : Attribute
        {
            #region Properties

            public string BuilderClassName { get; set; }
             
            #endregion
        }

        #endregion
    }
}