﻿using System;

namespace isukces.code.interfaces
{
    public partial class Auto
    {
        [AttributeUsage(AttributeTargets.Property)]
        public class WriteMemberAttribute : Attribute
        {
            #region Constructors

            public WriteMemberAttribute(string name)
            {
                Name = name;
            }

            #endregion

            #region Properties

            public string Name { get; private set; }

            #endregion
        }
    }
}