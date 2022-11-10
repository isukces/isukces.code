namespace iSukces.Code.Tests.Ui
{
    public class SampleStaticTextSource
    {
        public SampleStaticTextSource(string value) => Value = value;

        public string Value { get; }
    }

    public class SampleTranslatedTextSource
    {
        public SampleTranslatedTextSource(string translationKey)
        {
            TranslationKey = translationKey;
        }

        public string TranslationKey { get; }
    }
}