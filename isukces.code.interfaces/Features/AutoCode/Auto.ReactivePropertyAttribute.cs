using System;

namespace isukces.code.interfaces
{
    public partial class Auto
    {
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class ReactivePropertyAttribute : Attribute
        {
            #region Constructors

            public ReactivePropertyAttribute(string name, Type propertyType)
            {
                Name = name;
                PropertyType = propertyType;
            }

            #endregion

            #region Properties

            public string Name { get; set; }
            public Type PropertyType { get; set; }

            #endregion
        }
    }
}