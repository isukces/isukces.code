using System;

namespace iSukces.Code.Interfaces
{
    public partial class Auto
    {
        public class CopyBy
        {
            #region Nested

            [AttributeUsage(AttributeTargets.Property)]
            public class ReferenceAttribute : Attribute
            {
            }

            [AttributeUsage(AttributeTargets.Property)]
            public class ValuesProcessorAttribute : Attribute
            {
            }

            [AttributeUsage(AttributeTargets.Property)]
            public class CloneableAttribute : Attribute
            {
            }

            #endregion
        }
    }
}