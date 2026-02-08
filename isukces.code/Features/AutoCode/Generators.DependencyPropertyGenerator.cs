using System;
using iSukces.Code.Interfaces;
#if COREFX
#else
using System.Windows;
#endif

namespace iSukces.Code.AutoCode;

public partial class Generators
{
    internal class DependencyPropertyGenerator : SingleClassGeneratorMultiple<Auto.DependencyPropertyAttribute>, IAutoCodeGenerator
    {
        private static void Single(CsClass csClass, Auto.DependencyPropertyAttribute attribute)
        {
            var propertyTypeName = csClass.GetTypeName(attribute.PropertyType)
                .AsString(csClass.AllowReferenceNullable());
            var dpmi = new DependencyPropertyMetadata
            {
                PropertyChanged = attribute.PropertyChanged,
                DefaultValue    = attribute.DefaultValue,
                PropertyType    = attribute.PropertyType,
                Coerce          = attribute.Coerce
            };
            var fn = attribute.Name + "Property";

            var meta = dpmi.Resolve(attribute.Name, propertyTypeName);
            {
                var staticField = csClass.AddField(fn, new CsType("System.Windows.DependencyProperty"));
/*
# if COREFX
                    var staticField = csClass.AddField(fn, "System.Windows.DependencyProperty");
# else
                    var staticField = csClass.AddField(fn, typeof(System.Windows.DependencyProperty));
# endif
                    */
                staticField.IsStatic   = true;
                staticField.IsReadOnly = true;
                staticField.Visibility = Visibilities.Public;
                {
                    ICsCodeWriter writer = new CsCodeWriter();
                    // writer.WriteLine("public static readonly System.Windows.DependencyProperty {0}Property = ",i.Name);
                    writer.Indent++;
                    writer.WriteLine("System.Windows.DependencyProperty.Register(");
                    writer.Indent++;
                    writer.WriteLine("nameof({0}),", attribute.Name);
                    writer.WriteLine("typeof({0}), ", propertyTypeName);
                    if (string.IsNullOrEmpty(meta))
                        writer.WriteLine("{0})", csClass.Name.TypeOf());
                    else
                    {
                        writer.WriteLine("{0},", csClass.Name.TypeOf());
                        writer.WriteLine("{0})", meta);
                    }

                    staticField.ConstValue = writer.Code;
                }
            }

            {
                var f = csClass.AddProperty(attribute.Name, attribute.PropertyType);
                f.Visibility = Visibilities.Public;
                var ownGetter = string.Format("({0})GetValue({1});", propertyTypeName, fn);
                f.OwnGetter = PropertyGetterCode.ExpressionBody(ownGetter);

                var setter = string.Format("SetValue({0}Property, value);", attribute.Name);
                f.WithOwnSetterAsExpressionBody(setter);
                f.EmitField  = false;
                f.SetterType = PropertySetter.Set;
            }
        }

        protected override void GenerateInternal()
        {
            var csClass = Class;
            foreach (var attribute in Attributes)
            {
                Single(csClass, attribute);
            }
        }

        private class DependencyPropertyMetadata
        {
            public string? Resolve(string propertyName, string propertyTypeName)
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
                var coerceStr          = GetCoerceStr(propertyName);
                var initStr            = GetDefaultValueAsString(propertyTypeName);

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

            private string? GetCoerceStr(string propertyName)
            {
                var coerceStr = Coerce?.Trim();
                if (string.IsNullOrEmpty(coerceStr))
                    return null;
                return coerceStr == "*"
                    ? $"{propertyName}CoerceCallback"
                    : coerceStr;
            }

            private string? GetDefaultValueAsString(string propertyTypeName)
            {
                if (DefaultValue is null) return null;
                if (DefaultValue is bool boolValue && PropertyType == typeof(bool))
                    return boolValue ? "true" : "false";
                var initStr = DefaultValue?.ToString()?.Trim() ?? "";
                initStr = initStr == "*"
                    ? $"new {propertyTypeName}()"
                    : initStr.Replace("*", propertyTypeName);
                return initStr;
            }

            private string? GetPropertyChangedStr(string propertyName)
            {
                var propertyChangedStr = PropertyChanged?.Trim();
                if (string.IsNullOrEmpty(propertyChangedStr))
                    return null;
                return propertyChangedStr == "*"
                    ? $"On{propertyName}PropertyChanged"
                    : propertyChangedStr;
            }

            public string? PropertyChanged { get; set; }
            public object? DefaultValue    { get; set; }

            public Type    PropertyType { get; set; }
            public string? Coerce       { get; set; }
        }
    }
}