using System;
using iSukces.Translation;

namespace iSukces.Code.Translations
{
    internal sealed class CreateLiteLocalTextRequest : ICreateLiteLocalTextRequest
    {
        public CreateLiteLocalTextRequest(string key, Type proxyType2, string proxyPropertyName2, string sourceText)
        {
            Key              = key.Trim();
            FieldName        = proxyPropertyName2;
            SourceText       = sourceText;
            FieldHostingType = proxyType2;
        }

        public string GetLanguage()
        {
            return "polish";
        }

        public string GetSourceTextToTranslate()
        {
            return SourceText;
        }

        public string SourceText { get; }

        public string Key { get; }

        public string? TranslationHint  { get; set; }
        public string  FieldName        { get; }
        public Type    FieldHostingType { get; }
    }
}
