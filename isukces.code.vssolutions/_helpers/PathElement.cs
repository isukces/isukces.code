namespace isukces.code.vssolutions
{
    internal class PathElement
    {
        public static PathElement FromString(string s)
        {
            s = s?.Trim();
            if (string.IsNullOrEmpty(s))
                return null;
            return new PathElement
            {
                ElementName = s
            };
        }

        public string ElementName { get; set; }
    }
}