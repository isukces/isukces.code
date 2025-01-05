using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using iSukces.Code.Interfaces;
using iSukces.Translation;

namespace iSukces.Code.Translations
{
    public sealed class TranslationAutocodeConfig
    {
        static TranslationAutocodeConfig()
        {
            Instance = new TranslationAutocodeConfig();
        }

        public TranslationAutocodeConfig()
        {
            GroupProperties = new Dictionary<string, string>();
            VeryCommonNames = new Dictionary<string, string>();
        }

        private static IEnumerable<Tuple<string, string>> Split(string txt)
        {
            return txt.Split('\r', '\n')
                .Select(a => a.Split('='))
                .Where(a => a.Length == 2)
                .Select(a => new Tuple<string, string>(a[0].Trim(), a[1].Trim()));
        }


        public bool IsMainAssembly(Assembly assembly)
        {
            return MainAssembly == assembly;
        }

        public void RequestsAdd(ITranslationRequest? req)
        {
            switch (req)
            {
                case null: return;
                case ITranslationRequest k:
                    if (string.IsNullOrEmpty(k.Key))
                        return;
                    const StringComparison ordinal = StringComparison.Ordinal;
                    if (k.Key.StartsWith("Test.", ordinal)
                        || k.Key.StartsWith("Demo.", ordinal)
                        || k.Key.StartsWith("Ignore.", ordinal))
                        return;
                    break;
            }

            _requests.Add(req);
        }

        public delegate string GetInstanceHolderInstanceCsExpressionDelegate(ITypeNameResolver resolver);

        public static TranslationAutocodeConfig Instance { get; }

        public GetInstanceHolderInstanceCsExpressionDelegate? GetInstanceHolderInstanceCsExpression { get; set; }

        public IDictionary<string, string> VeryCommonNames { get; }
        public IDictionary<string, string> GroupProperties { get; }

        public Type? TranslationManager { get; set; }
        public Type? TranslationHolder  { get; set; }
        public Type? DefaultProxyType   { get; set; }
        public Type? CommonTranslations { get; set; }
        public Type? TranslationsInit   { get; set; }

        public Func<object, IEnumerable<CreateLiteLocalTextSources_Request>>? ConvertRequests { get; set; }

        public IAutocodeAssemblies? AutocodeAssemblies { get; set; }

        public Action<CsMethod, INamespaceContainer>? AddAppInitAttribute { get; set; }

        public Assembly? MainAssembly { get; set; }


        public Func<IEnumerable<ITranslationProxyCreateRequest>>? GetRequests { get; set; }


        public List<Action<CsMethodCodeWriter>> InitTranslationRequests { get; } =
            new List<Action<CsMethodCodeWriter>>();

        public IReadOnlyList<ITranslationRequest> Requests => _requests;

        private readonly List<ITranslationRequest> _requests = new List<ITranslationRequest>();
    }
}
