// ReSharper disable All
// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace iSukces.Code.Tests.LazyGenerator
{
    partial class LazyGeneratorTests
    {
        partial class SampleClass
        {
            public string OtherName()
            {
                var result = _xotherNameField;
                if (result == null)
                    lock(_sync)
                        if (result == null)
                            _xotherNameField = result = LazyText();
                return result;
            }

            public string OtherTextAsMethod()
            {
                if (ReferenceEquals(_otherTextAsMethod, null)) throw new System.Exception(LazyNotInitializedMessage);
                return _otherTextAsMethod.Value;
            }

            public int Value()
            {
                var result = _value;
                if (result == null)
                    lock(_sync)
                        if (result == null)
                            _value = result = new System.Tuple<int>(LazyValue());
                return result.Item1;
            }

            private void AutocodeInit()
            {
                _otherTextAsMethod = new System.Lazy<string>(LazyOtherTextAsMethod);
                _otherTextAsProperty = new System.Lazy<string>(LazyOtherTextAsProperty);
            }

            public string OtherTextAsProperty
            {
                get
                {
                    if (ReferenceEquals(_otherTextAsProperty, null)) throw new System.Exception(LazyNotInitializedMessage);
                    return _otherTextAsProperty.Value;
                }
            }

            private readonly object _sync = new object();

            private volatile System.Tuple<int> _value;

            private volatile string _xotherNameField;

            private System.Lazy<string> _otherTextAsMethod;

            private System.Lazy<string> _otherTextAsProperty;

            private const string LazyNotInitializedMessage = "Lazy not initialized. Call AutocodeInit method in constructor.";

        }

    }
}
