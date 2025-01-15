using System;
using iSukces.Code.Interfaces;

namespace iSukces.Code.AutoCode
{
    public partial class Generators
    {
        internal class ReactiveCommandGenerator : SingleClassGenerator
        {
            private static void Single(CsClass csClass, Auto.ReactiveCommandAttribute attribute)
            {
                // public ReactiveCommand<object> FullViewCommand { get; protected set; }
                //var type = $"ReactiveCommand<{csClass.GetTypeName(attribute.ResultType)}>";
                var type = new CsType("ReactiveCommand")
                    .WithGenericParameter(csClass.GetTypeName(attribute.ResultType));
                var p = csClass.AddProperty(attribute.Name + "Command", type);
                p.MakeAutoImplementIfPossible = true;
                p.Visibility                  = Visibilities.Public;
                p.SetterVisibility            = attribute.SetterVisibility;
                p.GetterVisibility            = attribute.GetterVisibility;
            }

            protected override void GenerateInternal()
            {
                var attributes = GetCustomAttributes<Auto.ReactiveCommandAttribute>(false);
                if (attributes is null || attributes.Length < 1) return;
                var csClass = Class;
                Context.AddNamespace("ReactiveUI");
                foreach (var attribute in attributes)
                    Single(csClass, attribute);
                {
                    var type = new CsType("IEnumerable")
                        .WithGenericParameter((CsType)"IReactiveCommand");
                    var m    = csClass.AddMethod("GetAllReactiveCommands", type, "");
                    var c    = new CsCodeWriter();
                    foreach (var attribute in attributes)
                        c.WriteLine("yield return {0}Command;", attribute.Name);
                    m.Body = c.Code;
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
                    if (DefaultValue is bool && PropetyType == typeof(bool))
                        return (bool)DefaultValue ? "true" : "false";
                    var initStr = DefaultValue.ToString().Trim();
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

                public string PropertyChanged { get; set; }
                public object DefaultValue    { get; set; }

                public Type   PropetyType { get; set; }
                public string Coerce      { get; set; }
            }
        }
    }
}
