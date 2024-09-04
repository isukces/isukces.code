using System;
using iSukces.Code.AutoCode;

namespace iSukces.Code.Interfaces;

public partial class Auto
{
    /// <summary>
    /// Used by <see cref="CopyFromGenerator">CopyFromGenerator</see>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
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