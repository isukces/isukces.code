using System;
using System.Diagnostics;

// ReSharper disable once CheckNamespace
namespace iSukces.Code.Interfaces;

public static partial class Auto
{
    /// <summary>
    /// Property or field decorated with this attribute will not be included
    /// in Equals nor GetHashCode method 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    [Conditional("AUTOCODE_ANNOTATIONS")]
    public class EqualityGeneratorSkipAttribute : Attribute
    {
            
    }
}