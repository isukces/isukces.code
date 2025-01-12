using System;

namespace iSukces.Code.AutoCode;

public sealed record CopyPropertyValueArgs(object Source, 
    object Target, 
    string PropertyName);



[AttributeUsage(AttributeTargets.Property)]
public sealed class CopyByReferenceAttribute : Attribute
{
        
}
