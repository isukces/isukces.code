using System;

namespace iSukces.Code.Interfaces
{
    public partial class Auto
    {
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class DependencyPropertyAttribute : Attribute
        {
            #region Constructors

            public DependencyPropertyAttribute(string name, Type propertyType)
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
