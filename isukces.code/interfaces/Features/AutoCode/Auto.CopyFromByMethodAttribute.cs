using System;
using System.Diagnostics;
using iSukces.Code.AutoCode;

namespace iSukces.Code.Interfaces;

public static partial class Auto
{
    /// <summary>
    /// Used by <see cref="CopyFromGenerator">CopyFromGenerator</see>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    [Conditional("AUTOCODE_ANNOTATIONS")]
    public class CopyFromByMethodAttribute : Attribute
    {
        public CopyFromByMethodAttribute(Type type, string methodName)
        {
            Type       = type;
            MethodName = methodName;
        }

        public Type   Type       { get; }
        public string MethodName { get;  }
    }
}
