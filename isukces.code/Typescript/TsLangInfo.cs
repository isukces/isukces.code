using iSukces.Code.Interfaces;

namespace iSukces.Code.Typescript;

public class TsLangInfo : ILangInfo
{
    private TsLangInfo()
    {
    }

    public static ILangInfo Instance => InstanceHolder.SingleInstance;

#if BOM
        /// <summary>
        ///     Dodaj BOM dla UTF-8
        /// </summary>
        public bool AddBOM => true;
#endif

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
        public static readonly TsLangInfo SingleInstance = new TsLangInfo();
    }
}
