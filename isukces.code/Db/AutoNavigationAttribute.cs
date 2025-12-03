using System;
using System.Diagnostics;

namespace iSukces.Code.Db;

[Conditional("AUTOCODE_ANNOTATIONS")]
public class AutoNavigationAttribute : Attribute
{
    public AutoNavigationAttribute(string name, Type type, string? inverse = null)
    {
        Name    = name;
        Type    = type;
        Inverse = inverse;
    }

    public AutoNavigationAttribute(Type type, string? inverse = null)
    {
        Type    = type;
        Inverse = inverse;
        Name    = "";
    }


    public string      Name                          { get; }
    public Type        Type                          { get; }
    public string?     Inverse                       { get; }
    public InverseKind GenerateInverse               { get; set; }
    public string?     NavigationPropertyDescription { get; set; }
}

public enum InverseKind
{
    None,
    Single,
    Collection
}
