using System;

namespace iSukces.Code.Translations
{
    public abstract class ProxyCreateRequest : TranslationTextSourceRequest,
        ITranslationProxyCreateRequest
    {
        protected ProxyCreateRequest(string key) : base(key)
        {
        }

        public string ProxyPropertyName     { get; protected set; }
        public Type   ProxyType             { get; protected set; }
        public bool   CanChangePropertyName { get; set; }
    }
}
