namespace isukces.code.Typescript
{
    public class TsMethodArgument
    {
        public TsMethodArgument()
        {
        }

        public TsMethodArgument(string name, string type)
        {
            Name = name;
            Type = type;
        }

        public string GetTsCode()
        {
            if (string.IsNullOrEmpty(Type))
                return Name;
            return Name + ": " + Type;
        }

        public string Name { get; set; }
        public string Type { get; set; }
    }
}