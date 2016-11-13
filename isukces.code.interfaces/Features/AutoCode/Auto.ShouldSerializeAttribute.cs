using System;

namespace isukces.code.interfaces
{
    public partial class Auto
    {
        [AttributeUsage(AttributeTargets.Property)]
        public class ShouldSerializeAttribute : Attribute
        {
            #region Constructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="T:System.Attribute" /> class.
            /// </summary>
            public ShouldSerializeAttribute(string condition)
            {
                Condition = condition;
            }

            public ShouldSerializeAttribute()
            {
            }

            #endregion

            #region Properties

            public string Condition { get; set; }

            #endregion
        }
    }
}