#region using

using System;

#endregion

namespace isukces.code.interfaces
{
    public partial class Auto
    {
        #region Nested

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class ReactivePropertyAttribute : PropertyAttributeBase
        {
            #region Constructors

            public ReactivePropertyAttribute(string name, Type propertyType, string description = null)
            {
                Name = name;
                PropertyType = propertyType;
                Description = description;
            }

            #endregion

            #region Properties

            public Visibilities FieldVisibility { get; set; } = Visibilities.Private;

            #endregion
        }

        #endregion
    }
}