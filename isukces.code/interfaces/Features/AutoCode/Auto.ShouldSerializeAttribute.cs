﻿#nullable enable
using System;

namespace iSukces.Code.Interfaces
{
    public partial class Auto
    {
        [AttributeUsage(AttributeTargets.Property)]
        public class ShouldSerializeAttribute : Attribute
        {
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

            public string Condition { get; set; }
        }
    }
}