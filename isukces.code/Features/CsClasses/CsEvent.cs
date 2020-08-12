namespace isukces.code
{
    public class CsEvent : ClassMemberBase
    {
        public CsEvent(string name, string type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }
        
        public string Type { get; }
    }
}