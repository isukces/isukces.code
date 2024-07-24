#nullable enable
namespace iSukces.Code;

public record struct CsPragmaWarning(Enabling Enabling, string Name)
{
    public static CsPragmaWarning Disable(string name) => new(Enabling.Disable, name);
    public static CsPragmaWarning Enable(string name) => new(Enabling.Enable, name);
}

public enum Enabling
{
    Disable, Enable
}
 