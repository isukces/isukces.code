namespace iSukces.Code.Typescript
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
            var opt = IsOptional ? "?" : "";
            if (string.IsNullOrEmpty(Type))
                return Name + opt;
            return $"{Name}{opt}: {Type}";
        }

        public string Name       { get; set; }
        public string Type       { get; set; }
        public bool   IsOptional { get; set; }
    }
}