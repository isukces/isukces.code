namespace isukces.code
{
    public class CsEvent : ClassMemberBase
    {
        public CsEvent(string name, string type, string description = null)
        {
            Name        = name;
            Type        = type;
            Description = description;
        }

        public string Name { get; }

        public string Type { get; }
    }
}