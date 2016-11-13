using System;

namespace isukces.code.interfaces
{
    public partial class Auto
    {
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class DependencyProperty : Attribute
        {
            #region Constructors

            public DependencyProperty(string name, Type propertyType)
            {
                Name = name;
                PropertyType = propertyType;
            }

            #endregion

            #region Properties

            public string Name { get; set; }
            public Type PropertyType { get; set; }

            public string PropertyChanged { get; set; }

            public object DefaultValue { get; set; }

            public string Coerce { get; set; }


            #endregion
        }
    }
}