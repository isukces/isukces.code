using System;

namespace isukces.code.interfaces
{
    public partial class Auto
    {
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class ReactiveCommandAttribute : Attribute
        {
            #region Constructors

            public ReactiveCommandAttribute(string name, Type resultType)
            {
                Name = name;
                ResultType = resultType;
            }

            #endregion

            #region Properties

            public string Name { get; set; }
            public Type ResultType { get; set; }

            #endregion
        }
    }
}