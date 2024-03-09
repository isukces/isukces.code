namespace iSukces.Code;

public class CsEvent : ClassMemberBase
{
    public CsEvent(string name, CsType type, string description = null)
    {
        Name        = name;
        Type        = type;
        Description = description;
        FieldName   = "_" + name.ToLowerInvariant();
    }

    public string Name { get; }

    public CsType Type           { get; set; }
    public string FieldName      { get; set; }
    public bool   LongDefinition { get; set; }

    public string GetFieldDescription()
    {
        return $"field for {Name} event";
        //return $"field for <see cref=\"{Name}\">{Name}</see> event";
    }
}