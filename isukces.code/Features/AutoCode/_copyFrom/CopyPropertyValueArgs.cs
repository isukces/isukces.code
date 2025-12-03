using System;
using System.Diagnostics;

namespace iSukces.Code.AutoCode;

public sealed record CopyPropertyValueArgs(object Source, 
    object Target, 
    string PropertyName);



[AttributeUsage(AttributeTargets.Property)]
[Conditional("AUTOCODE_ANNOTATIONS")]
public sealed class CopyByReferenceAttribute : Attribute
{
        
}
