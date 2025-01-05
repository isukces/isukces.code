using System.Collections.Generic;

namespace iSukces.Code.Translations
{
    public sealed class TranslationPropertiesDictionary
    {
        public static TranslationPropertiesDictionary GetOrCreate(IDictionary<string, object> dict)
        {
            var key = nameof(TranslationPropertiesDictionary);
            if (dict.TryGetValue(key, out var value))
                if (value is TranslationPropertiesDictionary r)
                    return r;

            var result = new TranslationPropertiesDictionary();
            dict[key] = result;
            return result;
        }

        public string ForgeProxyPropertyName(string proposedPropertyName, string fullKey)
        {
            if (_byKey.TryGetValue(fullKey, out var propertyName))
                return propertyName;

            if (_used.Add(proposedPropertyName))
                return _byKey[fullKey] = proposedPropertyName;

            var nr = 0;
            while (true)
            {
                propertyName = $"{proposedPropertyName}{++nr}";
                if (_used.Add(propertyName))
                    return _byKey[fullKey] = propertyName;
            }
        }

        public string PropertyForKey(string fullKey)
        {
            return _byKey.TryGetValue(fullKey, out var propertyName) ? propertyName : null;
        }

        public void Register(string key, string propertyName)
        {
            _byKey.Add(key, propertyName);
            _used.Add(propertyName);
        }

        public bool PropertyExists(string propName)
        {
            return _used.Contains(propName);
        }

        private readonly Dictionary<string, string> _byKey = new Dictionary<string, string>();
        private readonly HashSet<string> _used = new HashSet<string>();
    }
}