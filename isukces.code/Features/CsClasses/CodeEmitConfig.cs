namespace iSukces.Code;

public struct CodeEmitConfig
{
    public bool AllowReferenceNullable { get; init; }


    /// <summary>
    /// Indicates whether the generated property should support an associated backing field
    /// introduced in C# 14.
    /// </summary>
    public bool AllowPropertyBackField { get; init; }
}




