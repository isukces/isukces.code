using System;
using System.Diagnostics;

namespace iSukces.Code.Interfaces;

public static partial class Auto
{
    [Conditional("AUTOCODE_ANNOTATIONS")]
    public class BuilderAttribute : Attribute
    {
        public BuilderAttribute(string? builderClassName = null)
        {
            BuilderClassName = builderClassName;
        }

        public string? BuilderClassName { get; set; }
    }
}