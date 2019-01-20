namespace AutoCodeBuilder
{
    internal class FluentMethodInfo
    {
        public FluentMethodInfo(string name)
        {
            Name = name;
        }

        private string AddSuffix(string suffix)
        {
            if (IsFrom)
                return FluentPrefix + suffix;
            return FluentPrefix + "From" + suffix;
        }

        public bool AllowAncestor => IsFrom;

        private bool IsFrom => Name == "From";

        public string Name { get; }

        public string FluentPrefix
        {
            get
            {
                if (IsFrom)
                    return "WithBindFrom";
                return "With" + Name;
            }
        }

        public string Static => AddSuffix("Static");
        public string StaticResource => AddSuffix("Resource");
        public string Ancestor       => AddSuffix("Ancestor");
    }
}