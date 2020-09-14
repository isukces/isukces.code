using iSukces.Code.Interfaces;

namespace iSukces.Code
{
    public class CsLangInfo : ILangInfo
    {
        private CsLangInfo()
        {
        }

        public static ILangInfo Instance => InstanceHolder.SingleInstance;

        /// <summary>
        ///     Dodaj BOM dla UTF-8
        /// </summary>
        public bool AddBOM => true;

        /// <summary>
        ///     tekst zamykający
        /// </summary>
        public string CloseText => "}";

        /// <summary>
        ///     Komentarz jednolinijkowy
        /// </summary>
        public string OneLineComment => "//";

        /// <summary>
        ///     tekst otwierający
        /// </summary>
        public string OpenText => "{";

        private static class InstanceHolder
        {
            public static readonly CsLangInfo SingleInstance = new CsLangInfo();
        }
    }
}