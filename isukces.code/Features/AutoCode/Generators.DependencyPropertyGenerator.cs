using System;
using System.Linq;
using System.Reflection;
using iSukces.Code.CodeWrite;
using iSukces.Code.Interfaces;

namespace iSukces.Code.AutoCode
{
    public partial class Generators
    {
        internal class DependencyPropertyGenerator : SingleClassGeneratorMultiple<Auto.DependencyPropertyAttribute>, IAutoCodeGenerator
        {
            private static void Single(CsClass csClass, Auto.DependencyPropertyAttribute attribute)
            {
                var propertyTypeName = csClass.GetTypeName(attribute.PropertyType);
                var dpmi = new DependencyPropertyMetadata
                {
                    PropertyChanged = attribute.PropertyChanged,
                    DefaultValue = attribute.DefaultValue,
                    PropetyType = attribute.PropertyType,
                    Coerce = attribute.Coerce
                };
                var fn = attribute.Name + "Property";

                var meta = dpmi.Resolve(attribute.Name, propertyTypeName);
                {
#if COREFX
                    var staticField = csClass.AddField(fn, "System.Windows.DependencyProperty");
#else
                    var staticField = csClass.AddField(fn, typeof(System.Windows.DependencyProperty));
#endif
                    staticField.IsStatic = true;
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
                            writer.WriteLine("typeof({0}))", csClass.Name);
                        else
                        {
                            writer.WriteLine("typeof({0}),", csClass.Name);
                            writer.WriteLine("{0})", meta);
                        }
                        staticField.ConstValue = writer.Code;
                    }
                }

                {
                    var f = csClass.AddProperty(attribute.Name, attribute.PropertyType);
                    f.Visibility = Visibilities.Public;
                    f.OwnGetter = string.Format("({0})GetValue({1});", propertyTypeName, fn);
                    f.OwnGetterIsExpression = true;
                    f.OwnSetter = string.Format("SetValue({0}Property, value);", attribute.Name);
                    f.OwnSetterIsExpression = true;
                    f.EmitField = false;
                    f.IsPropertyReadOnly = false;
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
                    if (DefaultValue is bool boolValue && PropetyType == typeof(bool))
                        return boolValue ? "true" : "false";
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

                public string PropertyChanged { get; set; }
                public object DefaultValue { get; set; }

                public Type PropetyType { get; set; }
                public string Coerce { get; set; }
            }
        }
    }
}