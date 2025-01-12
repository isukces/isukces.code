using System;
using iSukces.Translation;

namespace iSukces.Code.Translations;

public abstract class TranslationTextSourceRequest : ITranslationTextSourceRequest
{
    protected TranslationTextSourceRequest(string key)
    {
        key = key?.Trim();
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));
        Key = key;
    }

    public abstract string GetLanguage();
    public abstract string GetSourceTextToTranslate();
    public string Key             { get; }
    public string TranslationHint { get; set; }
}