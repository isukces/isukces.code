using iSukces.Translation;

namespace iSukces.Code.Translations
{
    /// <summary>
    ///     Żądanie stworzenia autokodu z propertisami sięgającymi do tłumaczeń
    /// </summary>
    public interface ITranslationProxyCreateRequest : ITranslationProxyRequest
    {
        /// <summary>
        ///  czy może zmienić nazwę własności na bardziej odpowiadającą tłumaczeniu
        /// </summary>
        bool CanChangePropertyName { get; }
    }
}