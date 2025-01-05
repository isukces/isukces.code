using System;
using System.Reflection;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;
using iSukces.Translation;

namespace iSukces.Code.Translations
{
    public sealed class LocalTextSourceScanner : IAutoCodeGenerator
    {
        private static bool Accept(Type type)
        {
            return type == typeof(LocalTextSource) || type == typeof(LiteLocalTextSource);
        }

        private static void CreateInitCode(CsMethodCodeWriter writer, FieldInfo fieldInfo)
        {
            var fieldInfoDeclaringType = fieldInfo.DeclaringType ?? throw new InvalidOperationException();
            var target                 = writer.GetTypeName(fieldInfoDeclaringType).GetMemberCode(fieldInfo.Name);
            var instance               = TranslationAutocodeConfig.Instance.GetInstanceHolderInstanceCsExpression(writer);
            writer.WriteLine($"{target}.Attach({instance});");
        }

        public void Generate(Type type, IAutoCodeGeneratorContext context)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            var                requests     = TranslationAutocodeConfig.Instance.InitTranslationRequests;

            foreach (var fieldInfo in type.GetFields(bindingFlags))
            {
                if (fieldInfo.DeclaringType != fieldInfo.ReflectedType)
                    return;
                if (!Accept(fieldInfo.FieldType)) continue;
                requests.Add(writer => CreateInitCode(writer, fieldInfo));
            }
        }
    }
}
