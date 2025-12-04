using System;
using System.Diagnostics;

namespace iSukces.Code;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
[Conditional("AUTOCODE_ANNOTATIONS")]
public sealed class StringProcessingAttribute : Attribute
{
    public StringProcessingAttribute(StringProcessingTrim trim)
    {
        Trim = trim;
    }
    
    public StringProcessingTrim Trim { get; }

}

[Flags]
public enum StringProcessingTrim
{
    None = 0,
    NullIfEmpty = 1,
    TrimStart = 2,
    TrimEnd = 4,
    CoalesceNull = 8,
    RemoveDoubleWhiteChars = 16,

    Trim = TrimStart | TrimEnd,
    TrimCoalesceNull = Trim | CoalesceNull,
    NormalizeNotNull = Trim | CoalesceNull | RemoveDoubleWhiteChars
}
