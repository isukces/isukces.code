using System;
using System.Diagnostics;

namespace iSukces.Code.Interfaces;

public static partial class Auto
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    [Conditional("AUTOCODE_ANNOTATIONS")]
    public class ComparerGeneratorAttribute(params string[] fields) : Attribute
    {
        public string[] Fields { get; } = fields;
    }
}
