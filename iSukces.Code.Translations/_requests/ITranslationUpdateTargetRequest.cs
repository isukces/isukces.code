using iSukces.Translation;

namespace iSukces.Code.Translations;

/// <summary>
///     Żądanie zapisu wartości do określonego miejsca po zmianie tłumaczenia
/// </summary>
public interface ITranslationUpdateTargetRequest : ITranslationRequest
{
    string GetTarget(CsClass csClass);
}