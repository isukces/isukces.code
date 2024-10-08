﻿#nullable enable
#nullable enable
#nullable enable
#nullable enable
namespace iSukces.Code.Interfaces
{
    public interface ILangInfo
    {
        /// <summary>
        ///     Dodaj BOM dla UTF-8
        /// </summary>
        bool AddBOM { get; }

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
}