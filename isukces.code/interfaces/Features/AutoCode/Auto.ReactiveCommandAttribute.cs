using System;

namespace isukces.code.interfaces
{
    public partial class Auto
    {
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class ReactiveCommandAttribute : ClassMemberAttributeBase
        {
            #region Constructors

            public ReactiveCommandAttribute(string name, Type resultType, string description = null)
            {
                Name = name;
                ResultType = resultType;
                Description = description;
            }

            #endregion

            #region Properties
            public Type ResultType { get; private set; }

            public Visibilities? SetterVisibility { get; set; }
            public Visibilities? GetterVisibility { get; set; }
         
            #endregion
        }
    }
}