using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using iSukces.Code.AutoCode;
using iSukces.Code.Ui.DataGrid;
using iSukces.Reflection;
using iSukces.Translation;
using JetBrains.Annotations;

namespace iSukces.Code.Translations
{
    public   class LiteLocalTextSourceScannerBase : IAutoCodeGenerator 
    {
        protected virtual bool ProcessGridTranslations(object translationSource)
        {
            switch (translationSource)
            {
                
                case null:
                case string:
                    return true;
                default:
                    return false;
            }
        }
        
        private  void AddAutoGridTranslations(Type type)
        {
            if (type.IsAbstract || type.IsInterface)
                return;
            if (!typeof(BasicDataGridConfigurationProvider).IsAssignableFrom(type)) return;

            var t = (BasicDataGridConfigurationProvider)Activator.CreateInstance(type);
            foreach (var fi in t.GetColumnsGeneral())
            {
                var isSupported = ProcessGridTranslations(fi.HeaderSource);
                if (!isSupported)
                    throw new NotSupportedException("Unable to process translation for " + fi.HeaderSource?.GetType());
            }
        }

        protected virtual bool AcceptFieldName(string name)
        {
            return true;
        }
        
        public void Generate(Type type, IAutoCodeGeneratorContext context)
        {
            AddAutoGridTranslations(type);
            foreach (var fi in type.GetFields(flags))
            {
                if (fi.FieldType != typeof(LocalTextSource)) continue;
                if (!AcceptFieldName(fi.Name))
                    continue;
                var v = (LocalTextSource)fi.GetValue(null);
                var req = new CreateLiteLocalTextRequest(v.OriginalKey, type, fi.Name, v.OriginalText)
                {
                    TranslationHint = v.TranslationHint
                };
                TranslationAutocodeConfig.Instance.RequestsAdd(req);
            }

            var m = type
                .GetMethodsX(flags)
                .SingleOrDefault(a => a.Name == MethodName && a.GetParameters().Length == 0);
            if (m is null) return;
            
            
            var                                             result = m.Invoke(null, null);
            IEnumerable<CreateLiteLocalTextSources_Request> reqs;
            if (result is IEnumerable<CreateLiteLocalTextSources_Request> a)
                reqs = a;
            else
                reqs = TranslationAutocodeConfig.Instance.ConvertRequests(result);
            foreach (var v in reqs)
            {
                var req = new CreateLiteLocalTextRequest(v.Key, type, v.FieldName, v.OriginalText)
                {
                    TranslationHint = v.TranslationHint
                };
                TranslationAutocodeConfig.Instance.RequestsAdd(req);
            }
        }

        public const string MethodName = "GetCreateLiteLocalTextsRequests";
        private const BindingFlags flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
    }
}