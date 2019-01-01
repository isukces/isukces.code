namespace isukces.code.Wpf.Ammy
{
    public class AmmyLangInfo : ILangInfo
    {
        public bool AddBOM
        {
            get { return false; }
        }

        public string CloseText
        {
            get { return "}"; }
        }

        public string OneLineComment
        {
            get { return "//"; }
        }

        public string OpenText
        {
            get { return "{"; }
        }
    }
}