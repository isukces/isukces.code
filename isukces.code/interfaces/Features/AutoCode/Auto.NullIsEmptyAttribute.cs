using System;
using System.Diagnostics;

namespace iSukces.Code.Interfaces;

public static partial class Auto
{
    /// <summary>
    /// Treat null as string.Empty or collection without elements 
    /// </summary>
    [Conditional("AUTOCODE_ANNOTATIONS")]
    public class NullIsEmptyAttribute : Attribute
    {
        
    }
        
}