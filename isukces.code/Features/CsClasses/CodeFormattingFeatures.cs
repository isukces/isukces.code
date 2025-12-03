using System;

namespace iSukces.Code;

[Flags]
public enum CodeFormattingFeatures
{
    None = 0,
    ExpressionBody = 1,
    Regions = 2,

    /// <summary>
    /// Allow 'is not null' 
    /// </summary>
    IsNotNull = 4,

    MakeAutoImplementIfPossible = 8,

    /// <summary>
    /// i.e. dict.TryGetValue(key, out var text)
    /// </summary>
    OutVar = 16,


    PropertyBackField = 32,

    Cs7 = OutVar,
    Cs12 = ExpressionBody | IsNotNull | MakeAutoImplementIfPossible | Cs7,
    Cs14 = Cs12 | PropertyBackField
}

public struct CodeFormatting
{
    public CodeFormatting(CodeFormattingFeatures flags, int maxLineLength)
    {
        Flags         = flags;
        MaxLineLength = maxLineLength;
    }

    public        CodeFormattingFeatures Flags         { get; }
    public        int                    MaxLineLength { get; }
    public static int                    IndentSpaces  { get; set; } = 4;

    public CodeFormatting With(CodeFormattingFeatures flag)
    {
        return new CodeFormatting(Flags | flag, MaxLineLength);
    }
}
