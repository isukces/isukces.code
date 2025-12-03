using System;
using System.Diagnostics;

namespace iSukces.Code.Interfaces;

public static partial class Auto
{
    [AttributeUsage(AttributeTargets.Constructor)]
    [Conditional("AUTOCODE_ANNOTATIONS")]
    public class CloneableConstructorAttribute : Attribute
    {
            
    }
}