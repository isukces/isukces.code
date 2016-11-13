namespace isukces.code
{
    public class CSLangInfo: object, ILangInfo {
        #region Properties
        /// <summary>
        /// Dodaj BOM dla UTF-8
        /// </summary>
        public bool AddBOM => true;

        /// <summary>
        /// tekst zamykający
        /// </summary>
        public string CloseText => "}";

        /// <summary>
        /// Komentarz jednolinijkowy
        /// </summary>
        public string OneLineComment => "//";

        /// <summary>
        /// tekst otwierający
        /// </summary>
        public string OpenText => "{";

        #endregion

    }
}
