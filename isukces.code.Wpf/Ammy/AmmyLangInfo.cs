using isukces.code.interfaces;

namespace isukces.code.Wpf.Ammy
{
    public class AmmyLangInfo : ILangInfo
    {
        private AmmyLangInfo()
        {
        }

        public static ILangInfo Instance => InstanceHolder.SingleInstance;


        public bool AddBOM => false;

        public string CloseText => "}";

        public string OneLineComment => "//";

        public string OpenText => "{";

        private static class InstanceHolder
        {
            public static readonly AmmyLangInfo SingleInstance = new AmmyLangInfo();
        }
    }
}