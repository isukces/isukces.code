using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using isukces.code.CodeWrite;
using isukces.code.interfaces;

namespace isukces.code.AutoCode
{
    internal partial class Generators
    {
        internal class ReactivePropertyGenerator : SingleClassGenerator, IAutoCodeGenerator
        {

            #region Static Methods

            private static void Single(CsClass csClass, Auto.ReactivePropertyAttribute attribute)
            {
                var p = csClass.AddProperty(attribute.Name, attribute.PropertyType);
                p.Description = attribute.Description;
                p.EmitField = true;
                p.OwnGetter = $"return {p.PropertyFieldName};";
                p.OwnSetter = $"this.RaiseAndSetIfChanged(ref {p.PropertyFieldName}, value);";

                p.SetterVisibility = attribute.SetterVisibility;
                p.GetterVisibility = attribute.GetterVisibility;
                p.FieldVisibility = attribute.FieldVisibility;
            }

            #endregion

            #region Instance Methods

            internal void GenerateInternal()
            {
                var attributes = Type.GetCustomAttributes<Auto.ReactivePropertyAttribute>(false).ToArray();
                if (attributes == null || attributes.Length < 1) return;
                var csClass = Class;
                Context.AddNamespace("ReactiveUI");
                foreach (var attribute in attributes)
                {
                    Single(csClass, attribute);
                }
            }

            #endregion

            #region Nested

            private class DependencyPropertyMetadata
            {
                #region Instance Methods

                public string Resolve(string propertyName, string propertyTypeName)
                {
                    /*
                        new FrameworkPropertyMetadata(
            Double.NaN,
            FrameworkPropertyMetadataOptions.AffectsMeasure,
            new PropertyChangedCallback(OnCurrentReadingChanged),
            new CoerceValueCallback(CoerceCurrentReading)

                        {
            public PropertyMetadata();
            public PropertyMetadata(object defaultValue);
            public PropertyMetadata(PropertyChangedCallback propertyChangedCallback);
            public PropertyMetadata(object defaultValue, PropertyChangedCallback propertyChangedCallback);
            public PropertyMetadata(object defaultValue, PropertyChangedCallback propertyChangedCallback, CoerceValueCallback coerceValueCallback);

            ),

                     */
                    var propertyChangedStr = GetPropertyChangedStr(propertyName);
                    var coerceStr = GetCoerceStr(propertyName);
                    var initStr = GetDefaultValueAsString(propertyTypeName);

                    if (!string.IsNullOrEmpty(coerceStr))
                    {
                        if (string.IsNullOrEmpty(initStr))
                            initStr = "default(" + propertyTypeName + ")";
                        if (string.IsNullOrEmpty(propertyChangedStr))
                            propertyChangedStr = "null";
                        return $"new System.Windows.PropertyMetadata({initStr}, {propertyChangedStr}, {coerceStr})";
                    }
                    if (string.IsNullOrEmpty(initStr))
                        return string.IsNullOrEmpty(propertyChangedStr)
                            ? null
                            : $"new System.Windows.PropertyMetadata({propertyChangedStr})";
                    return string.IsNullOrEmpty(propertyChangedStr)
                        ? $"new System.Windows.PropertyMetadata({initStr})"
                        : $"new System.Windows.PropertyMetadata({initStr}, {propertyChangedStr})";
                }

                private string GetCoerceStr(string propertyName)
                {
                    var coerceStr = Coerce?.Trim();
                    if (string.IsNullOrEmpty(coerceStr))
                        return null;
                    return coerceStr == "*"
                        ? $"{propertyName}CoerceCallback"
                        : coerceStr;
                }

                private string GetDefaultValueAsString(string propertyTypeName)
                {
                    if (DefaultValue == null) return null;
                    if (DefaultValue is bool && (PropetyType == typeof(bool)))
                        return (bool)DefaultValue ? "true" : "false";
                    var initStr = DefaultValue.ToString().Trim();
                    initStr = initStr == "*"
                        ? $"new {propertyTypeName}()"
                        : initStr.Replace("*", propertyTypeName);
                    return initStr;
                }

                private string GetPropertyChangedStr(string propertyName)
                {
                    var propertyChangedStr = PropertyChanged?.Trim();
                    if (string.IsNullOrEmpty(propertyChangedStr))
                        return null;
                    return propertyChangedStr == "*"
                        ? $"On{propertyName}PropertyChanged"
                        : propertyChangedStr;
                }

                #endregion

                #region Properties

                public string PropertyChanged { get; set; }
                public object DefaultValue { get; set; }

                public Type PropetyType { get; set; }
                public string Coerce { get; set; }

                #endregion
            }

            #endregion

            public void Generate(Type type, IAutoCodeGeneratorContext context)
            {
                Setup(type, context);
                GenerateInternal();
            }
        }
    }
}