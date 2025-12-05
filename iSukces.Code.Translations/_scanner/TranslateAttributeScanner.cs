using System;
using System.Diagnostics;
using System.Reflection;
using iSukces.Code.AutoCode;
using iSukces.Translation;

namespace iSukces.Code.Translations
{
    public sealed class TranslateAttributeScanner : IAutoCodeGenerator
    {
        private static TranslateAttribute? Get(Type t) 
        {
            #if DEBUG
            if (t.Name == "Trans1")
                Debug.WriteLine("");
            #endif
            if (t.GetCustomAttribute<System.Runtime.CompilerServices.CompilerGeneratedAttribute>() is not null) 
                return null;
            var path = TranslationUtils.GetTranslationKeyFromType(t);
            if (path is null)
                return null;
            var at = t?.GetCustomAttribute<TranslateAttribute>();
            if (at != null)
                return new TranslateAttribute(path.Path, at.Language);
            if (t?.DeclaringType is null)
                return new TranslateAttribute(path.Path);
            at = Get(t.DeclaringType);
            return new TranslateAttribute(path.Path, at?.Language);
        }

        private void Add(MemberInfo memberInfo, TranslateAttribute? classAttribute)
        {
            if (memberInfo.DeclaringType != memberInfo.ReflectedType)
                return;
            var cg = memberInfo.GetCustomAttribute<System.Runtime.CompilerServices.CompilerGeneratedAttribute>();
            if (cg is not null)
                return;
            var myAttribute = memberInfo.GetCustomAttribute<TranslateAttribute>();
            if (classAttribute is null) return;
            var mt = UpdateTextRequest.GetResultType(memberInfo);
            if (mt != typeof(string))
                return;
            if (memberInfo is FieldInfo fieldInfo)
                if (fieldInfo.IsLiteral)
                    return;

            var req = new UpdateTextRequest(memberInfo,
                myAttribute?.Key,
                myAttribute?.Language ?? classAttribute.Language,
                ClassMemberKeyPrefixFinder);
            TranslationAutocodeConfig.Instance.RequestsAdd(req);
        }

        private string ClassMemberKeyPrefixFinder(Type reflectedType)
        {
            var key = TranslationUtils.GetTranslationKeyFromType(reflectedType);
            return key is not null ? key.Path : DefaultClassMemberKeyPrefix;
        }

        public void Generate(Type type, IAutoCodeGeneratorContext? context)
        {
#if DEBUGx
            if (type.Name=="ConectDotsTranslations")
                Debug.WriteLine("");
#endif
            const BindingFlags bindingFlags   = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            var                classAttribute = Get(type);
            foreach (var fieldInfo in type.GetFields(bindingFlags))
                Add(fieldInfo, classAttribute);
            foreach (var propertyInfo in type.GetPropertiesX(bindingFlags))
                Add(propertyInfo, classAttribute);
        }

        public string DefaultClassMemberKeyPrefix { get; set; } = "Default";


        // [UmlDiagram(UmlDiagrams.Translations)]
        private sealed class UpdateTextRequest : ITranslationTextSourceRequest,
            ITranslationUpdateTargetRequest
        {
            public UpdateTextRequest(MemberInfo member, string forceKey, string language, ClassMemberKeyPrefixFinderDelegate classMemberKeyPrefixFinder)
            {
                Language       = language;
                _keyNamePrefix = classMemberKeyPrefixFinder(member.ReflectedType);
                _member        = member;
                _forceKey      = forceKey;

                if (GetResultType(member) != typeof(string))
                    throw new Exception("Value should be string");
            }

            public static Type GetResultType(MemberInfo member)
            {
                return member switch
                {
                    FieldInfo fi => fi.FieldType,
                    PropertyInfo pi => pi.PropertyType,
                    _ => throw new NotSupportedException()
                };
            }

            public string GetLanguage()
            {
                if (string.IsNullOrEmpty(Language))
                    return "polish";
                return Language;
            }

            public string GetSourceTextToTranslate()
            {
                var text= _member switch
                {
                    FieldInfo fi => (string?)fi.GetValue(null),
                    PropertyInfo pi => (string?)pi.GetValue(null),
                    _ => throw new NotSupportedException()
                };
                return text ?? string.Empty;
            }

            public string GetTarget(CsClass csClass)
            {
                return csClass.GetTypeName(_member.DeclaringType).GetMemberCode(_member.Name);
            }

            public string Language { get; }


            private string ShortKey
            {
                get
                {
                    if (!string.IsNullOrEmpty(_forceKey))
                        return _forceKey;
                    return TranslationUtils.GetTranslationKeyFromMember(_member)?.Path;
                }
            }

            public string TranslationHint { get; set; }

            public string Key => _keyNamePrefix + "." + ShortKey;

            private readonly string _keyNamePrefix;
            private readonly MemberInfo _member;
            private readonly string _forceKey;
        }
    }

    public delegate string ClassMemberKeyPrefixFinderDelegate(Type? reflectedType);
}
