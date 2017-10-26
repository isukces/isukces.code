namespace isukces.code
{
    public class CSLangInfo: object, ILangInfo {
        /// <summary>
        /// Dodaj BOM dla UTF-8
        /// </summary>
        public bool AddBOM
        {
            get { return true; }
        }

        /// <summary>
        /// tekst zamykający
        /// </summary>
        public string CloseText
        {
            get { return "}"; }
        }

        /// <summary>
        /// Komentarz jednolinijkowy
        /// </summary>
        public string OneLineComment
        {
            get { return "//"; }
        }

        /// <summary>
        /// tekst otwierający
        /// </summary>
        public string OpenText
        {
            get { return "{"; }
        }
    }
}
