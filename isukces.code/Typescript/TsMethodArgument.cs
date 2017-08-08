namespace isukces.code.Typescript
{
    public class TsMethodArgument
    {
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