#nullable enable

namespace iSukces.Code.Interfaces;

public interface ILangInfo
{
#if BOM
    /// <summary>
    ///     Dodaj BOM dla UTF-8
    /// </summary>
    bool AddBOM { get; }
#endif

    /// <summary>
    ///     tekst zamykający
    /// </summary>
    string CloseText { get; }

    /// <summary>
    ///     Komentarz jednolinijkowy
    /// </summary>
    string OneLineComment { get; }

    /// <summary>
    ///     tekst otwierający
    /// </summary>
    string OpenText { get; }
}