namespace iSukces.Code
{
    public class CsEvent : ClassMemberBase
    {
        public CsEvent(string name, string type, string description = null)
        {
            Name        = name;
            Type        = type;
            Description = description;
            FieldName   = "_" + name.ToLowerInvariant();
        }

        public string Name { get; }

        public string Type           { get; set; }
        public string FieldName      { get; set; }
        public bool   LongDefinition { get; set; }

        public string GetFieldDescription()
        {
            return $"field for {Name} event";
            //return $"field for <see cref=\"{Name}\">{Name}</see> event";
        }
    }
}