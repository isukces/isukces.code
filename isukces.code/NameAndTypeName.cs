
namespace iSukces.Code;

public readonly struct NameAndTypeName
{
    public NameAndTypeName(string name, CsType type)
    {
        Name = name;
        Type = type;
    }

    public string Name { get; }
    public CsType Type { get; }
}