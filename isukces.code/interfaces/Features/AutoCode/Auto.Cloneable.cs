using System;
using System.Diagnostics;
using iSukces.Code.AutoCode;

namespace iSukces.Code.Interfaces;

public static partial class Auto
{
    /// <summary>
    /// Used by <see cref="CopyFromGenerator">CopyFromGenerator</see>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    [Conditional("AUTOCODE_ANNOTATIONS")]
    public class CloneableAttribute : Attribute
    {
    }
}