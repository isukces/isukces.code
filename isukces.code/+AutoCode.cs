// ReSharper disable All
using iSukces.Code;
using iSukces.Code.Ammy;
using iSukces.Code.Interfaces.Ammy;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace iSukces.Code.Ammy
{
    partial class AmmyBind
    {
        [AutocodeGenerated]
        public AmmyBind WithBindFrom(object from)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            From = from; return this;
        }

        [AutocodeGenerated]
        public AmmyBind WithBindFromAncestor(System.Type ancestorType, int? level = null)
        {
            // generator : FluentBindGenerator.AddFluentMethod:196
            From = new AncestorBindingSource(ancestorType, level); return this;
        }

        [AutocodeGenerated]
        public AmmyBind WithBindFromAncestor<TAncestor>(int? level = null)
        {
            // generator : FluentBindGenerator.AddFluentMethod:207
            From = new AncestorBindingSource(typeof(TAncestor),  level); return this;
        }

        [AutocodeGenerated]
        public AmmyBind WithBindFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:146
            From = new AmmyDynamicResource(dynamicResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBind WithBindFromElementName(string elementName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:156
            From = new ElementNameBindingSource(elementName); return this;
        }

        [AutocodeGenerated]
        public AmmyBind WithBindFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:135
            From = new AmmyStaticResource(staticResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBind WithBindFromSelf()
        {
            // generator : FluentBindGenerator.AddFluentMethod:169
            From = SelfBindingSource.Instance; return this;
        }

        [AutocodeGenerated]
        public AmmyBind WithBindFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:125
            From = new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName); return this;
        }

        [AutocodeGenerated]
        public AmmyBind WithBindingGroupName(string bindingGroupName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("BindingGroupName", bindingGroupName);
        }

        [AutocodeGenerated]
        public AmmyBind WithBindingGroupName(object bindingGroupName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("BindingGroupName", bindingGroupName);
        }

        [AutocodeGenerated]
        public AmmyBind WithBindsDirectlyToSource(bool bindsDirectlyToSource)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("BindsDirectlyToSource", bindsDirectlyToSource);
        }

        [AutocodeGenerated]
        public AmmyBind WithBindsDirectlyToSource(object bindsDirectlyToSource)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("BindsDirectlyToSource", bindsDirectlyToSource);
        }

        [AutocodeGenerated]
        public AmmyBind WithConverter(object converter)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("Converter", converter);
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterCulture(object converterCulture)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("ConverterCulture", converterCulture);
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterCultureFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:146
            return WithSetParameter("ConverterCulture", new AmmyDynamicResource(dynamicResourceName));
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterCultureFromElementName(string elementName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:156
            return WithSetParameter("ConverterCulture", new ElementNameBindingSource(elementName));
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterCultureFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:135
            return WithSetParameter("ConverterCulture", new AmmyStaticResource(staticResourceName));
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterCultureFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:125
            return WithSetParameter("ConverterCulture", new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName));
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:146
            return WithSetParameter("Converter", new AmmyDynamicResource(dynamicResourceName));
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterFromElementName(string elementName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:156
            return WithSetParameter("Converter", new ElementNameBindingSource(elementName));
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:135
            return WithSetParameter("Converter", new AmmyStaticResource(staticResourceName));
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:125
            return WithSetParameter("Converter", new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName));
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterParameter(object converterParameter)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("ConverterParameter", converterParameter);
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterParameterFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:146
            return WithSetParameter("ConverterParameter", new AmmyDynamicResource(dynamicResourceName));
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterParameterFromElementName(string elementName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:156
            return WithSetParameter("ConverterParameter", new ElementNameBindingSource(elementName));
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterParameterFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:135
            return WithSetParameter("ConverterParameter", new AmmyStaticResource(staticResourceName));
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterParameterFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:125
            return WithSetParameter("ConverterParameter", new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName));
        }

        [AutocodeGenerated]
        public AmmyBind WithIsAsync(bool isAsync)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("IsAsync", isAsync);
        }

        [AutocodeGenerated]
        public AmmyBind WithIsAsync(object isAsync)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("IsAsync", isAsync);
        }

        [AutocodeGenerated]
        public AmmyBind WithMode(iSukces.Code.Compatibility.System.Windows.Data.XBindingMode mode)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("Mode", mode);
        }

        [AutocodeGenerated]
        public AmmyBind WithMode(object mode)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("Mode", mode);
        }

        [AutocodeGenerated]
        public AmmyBind WithNotifyOnSourceUpdated(bool notifyOnSourceUpdated)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("NotifyOnSourceUpdated", notifyOnSourceUpdated);
        }

        [AutocodeGenerated]
        public AmmyBind WithNotifyOnSourceUpdated(object notifyOnSourceUpdated)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("NotifyOnSourceUpdated", notifyOnSourceUpdated);
        }

        [AutocodeGenerated]
        public AmmyBind WithNotifyOnTargetUpdated(bool notifyOnTargetUpdated)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("NotifyOnTargetUpdated", notifyOnTargetUpdated);
        }

        [AutocodeGenerated]
        public AmmyBind WithNotifyOnTargetUpdated(object notifyOnTargetUpdated)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("NotifyOnTargetUpdated", notifyOnTargetUpdated);
        }

        [AutocodeGenerated]
        public AmmyBind WithNotifyOnValidationError(bool notifyOnValidationError)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("NotifyOnValidationError", notifyOnValidationError);
        }

        [AutocodeGenerated]
        public AmmyBind WithNotifyOnValidationError(object notifyOnValidationError)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("NotifyOnValidationError", notifyOnValidationError);
        }

        [AutocodeGenerated]
        public AmmyBind WithPath(string path)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            Path = path; return this;
        }

        [AutocodeGenerated]
        public AmmyBind WithStringFormat(string stringFormat)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("StringFormat", stringFormat);
        }

        [AutocodeGenerated]
        public AmmyBind WithStringFormat(object stringFormat)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("StringFormat", stringFormat);
        }

        [AutocodeGenerated]
        public AmmyBind WithTargetNullValue(object targetNullValue)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("TargetNullValue", targetNullValue);
        }

        [AutocodeGenerated]
        public AmmyBind WithUpdateSourceTrigger(iSukces.Code.Compatibility.System.Windows.Data.XUpdateSourceTrigger updateSourceTrigger)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("UpdateSourceTrigger", updateSourceTrigger);
        }

        [AutocodeGenerated]
        public AmmyBind WithUpdateSourceTrigger(object updateSourceTrigger)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("UpdateSourceTrigger", updateSourceTrigger);
        }

        [AutocodeGenerated]
        public AmmyBind WithValidatesOnDataErrors(bool validatesOnDataErrors)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("ValidatesOnDataErrors", validatesOnDataErrors);
        }

        [AutocodeGenerated]
        public AmmyBind WithValidatesOnDataErrors(object validatesOnDataErrors)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("ValidatesOnDataErrors", validatesOnDataErrors);
        }

        [AutocodeGenerated]
        public AmmyBind WithValidatesOnExceptions(bool validatesOnExceptions)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("ValidatesOnExceptions", validatesOnExceptions);
        }

        [AutocodeGenerated]
        public AmmyBind WithValidatesOnExceptions(object validatesOnExceptions)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("ValidatesOnExceptions", validatesOnExceptions);
        }

        [AutocodeGenerated]
        public AmmyBind WithValidatesOnNotifyDataErrors(bool validatesOnNotifyDataErrors)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("ValidatesOnNotifyDataErrors", validatesOnNotifyDataErrors);
        }

        [AutocodeGenerated]
        public AmmyBind WithValidatesOnNotifyDataErrors(object validatesOnNotifyDataErrors)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("ValidatesOnNotifyDataErrors", validatesOnNotifyDataErrors);
        }

        [AutocodeGenerated]
        public AmmyBind WithValidationRules(object validationRules)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("ValidationRules", validationRules);
        }

        [AutocodeGenerated]
        public AmmyBind WithXPath(string xPath)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("XPath", xPath);
        }

        [AutocodeGenerated]
        public AmmyBind WithXPath(object xPath)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            return WithSetParameter("XPath", xPath);
        }

        [AutocodeGenerated]
        public AmmyBind WithXPathFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:146
            return WithSetParameter("XPath", new AmmyDynamicResource(dynamicResourceName));
        }

        [AutocodeGenerated]
        public AmmyBind WithXPathFromElementName(string elementName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:156
            return WithSetParameter("XPath", new ElementNameBindingSource(elementName));
        }

        [AutocodeGenerated]
        public AmmyBind WithXPathFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:135
            return WithSetParameter("XPath", new AmmyStaticResource(staticResourceName));
        }

        [AutocodeGenerated]
        public AmmyBind WithXPathFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:125
            return WithSetParameter("XPath", new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName));
        }

    }

    partial class AmmyBindBuilder
    {
        [AutocodeGenerated]
        public AmmyBindBuilder WithBindFrom(object from)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            From = from; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithBindFromAncestor(System.Type ancestorType, int? level = null)
        {
            // generator : FluentBindGenerator.AddFluentMethod:196
            From = new AncestorBindingSource(ancestorType, level); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithBindFromAncestor<TAncestor>(int? level = null)
        {
            // generator : FluentBindGenerator.AddFluentMethod:207
            From = new AncestorBindingSource(typeof(TAncestor),  level); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithBindFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:146
            From = new AmmyDynamicResource(dynamicResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithBindFromElementName(string elementName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:156
            From = new ElementNameBindingSource(elementName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithBindFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:135
            From = new AmmyStaticResource(staticResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithBindFromSelf()
        {
            // generator : FluentBindGenerator.AddFluentMethod:169
            From = SelfBindingSource.Instance; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithBindFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:125
            From = new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithBindingGroupName(string bindingGroupName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            BindingGroupName = bindingGroupName; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithBindsDirectlyToSource(bool? bindsDirectlyToSource)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            BindsDirectlyToSource = bindsDirectlyToSource; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverter(object converter)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            Converter = converter; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterCulture(object converterCulture)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            ConverterCulture = converterCulture; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterCultureFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:146
            ConverterCulture = new AmmyDynamicResource(dynamicResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterCultureFromElementName(string elementName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:156
            ConverterCulture = new ElementNameBindingSource(elementName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterCultureFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:135
            ConverterCulture = new AmmyStaticResource(staticResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterCultureFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:125
            ConverterCulture = new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:146
            Converter = new AmmyDynamicResource(dynamicResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterFromElementName(string elementName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:156
            Converter = new ElementNameBindingSource(elementName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:135
            Converter = new AmmyStaticResource(staticResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:125
            Converter = new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterParameter(object converterParameter)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            ConverterParameter = converterParameter; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterParameterFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:146
            ConverterParameter = new AmmyDynamicResource(dynamicResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterParameterFromElementName(string elementName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:156
            ConverterParameter = new ElementNameBindingSource(elementName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterParameterFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:135
            ConverterParameter = new AmmyStaticResource(staticResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterParameterFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:125
            ConverterParameter = new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithIsAsync(bool? isAsync)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            IsAsync = isAsync; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithMode(iSukces.Code.Compatibility.System.Windows.Data.XBindingMode? mode)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            Mode = mode; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithNotifyOnSourceUpdated(bool? notifyOnSourceUpdated)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            NotifyOnSourceUpdated = notifyOnSourceUpdated; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithNotifyOnTargetUpdated(bool? notifyOnTargetUpdated)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            NotifyOnTargetUpdated = notifyOnTargetUpdated; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithNotifyOnValidationError(bool? notifyOnValidationError)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            NotifyOnValidationError = notifyOnValidationError; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithPath(string path)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            Path = path; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithStringFormat(string stringFormat)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            StringFormat = stringFormat; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithTargetNullValue(object targetNullValue)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            TargetNullValue = targetNullValue; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithTargetNullValueFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:146
            TargetNullValue = new AmmyDynamicResource(dynamicResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithTargetNullValueFromElementName(string elementName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:156
            TargetNullValue = new ElementNameBindingSource(elementName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithTargetNullValueFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:135
            TargetNullValue = new AmmyStaticResource(staticResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithTargetNullValueFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:125
            TargetNullValue = new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithUpdateSourceTrigger(iSukces.Code.Compatibility.System.Windows.Data.XUpdateSourceTrigger? updateSourceTrigger)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            UpdateSourceTrigger = updateSourceTrigger; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithValidatesOnDataErrors(bool? validatesOnDataErrors)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            ValidatesOnDataErrors = validatesOnDataErrors; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithValidatesOnExceptions(bool? validatesOnExceptions)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            ValidatesOnExceptions = validatesOnExceptions; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithValidatesOnNotifyDataErrors(bool? validatesOnNotifyDataErrors)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            ValidatesOnNotifyDataErrors = validatesOnNotifyDataErrors; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithXPath(string xPath)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            XPath = xPath; return this;
        }

        [AutocodeGenerated]
        private void SetupAmmyBind(AmmyBind bind)
        {
            // generator : FluentBindGenerator.BuildPropertiesAndFluentMethods:250
            if (Mode != null)
                bind.WithMode(Mode);
            if (Converter != null)
                bind.WithConverter(Converter);
            if (ConverterCulture != null)
                bind.WithConverterCulture(ConverterCulture);
            if (ConverterParameter != null)
                bind.WithConverterParameter(ConverterParameter);
            if (UpdateSourceTrigger != null)
                bind.WithUpdateSourceTrigger(UpdateSourceTrigger);
            if (NotifyOnSourceUpdated != null)
                bind.WithNotifyOnSourceUpdated(NotifyOnSourceUpdated);
            if (NotifyOnTargetUpdated != null)
                bind.WithNotifyOnTargetUpdated(NotifyOnTargetUpdated);
            if (NotifyOnValidationError != null)
                bind.WithNotifyOnValidationError(NotifyOnValidationError);
            if (ValidatesOnExceptions != null)
                bind.WithValidatesOnExceptions(ValidatesOnExceptions);
            if (ValidatesOnDataErrors != null)
                bind.WithValidatesOnDataErrors(ValidatesOnDataErrors);
            if (ValidatesOnNotifyDataErrors != null)
                bind.WithValidatesOnNotifyDataErrors(ValidatesOnNotifyDataErrors);
            if (StringFormat != null)
                bind.WithStringFormat(StringFormat);
            if (BindingGroupName != null)
                bind.WithBindingGroupName(BindingGroupName);
            if (TargetNullValue != null)
                bind.WithTargetNullValue(TargetNullValue);
            if (BindsDirectlyToSource != null)
                bind.WithBindsDirectlyToSource(BindsDirectlyToSource);
            if (IsAsync != null)
                bind.WithIsAsync(IsAsync);
            if (XPath != null)
                bind.WithXPath(XPath);
            if (From != null)
                bind.WithBindFrom(From);
            if (Path != null)
                bind.WithPath(Path);
        }

        [AutocodeGenerated]
        public iSukces.Code.Compatibility.System.Windows.Data.XBindingMode? Mode { get; set; }

        [AutocodeGenerated]
        public object Converter { get; set; }

        [AutocodeGenerated]
        public object ConverterCulture { get; set; }

        [AutocodeGenerated]
        public object ConverterParameter { get; set; }

        [AutocodeGenerated]
        public iSukces.Code.Compatibility.System.Windows.Data.XUpdateSourceTrigger? UpdateSourceTrigger { get; set; }

        [AutocodeGenerated]
        public bool? NotifyOnSourceUpdated { get; set; }

        [AutocodeGenerated]
        public bool? NotifyOnTargetUpdated { get; set; }

        [AutocodeGenerated]
        public bool? NotifyOnValidationError { get; set; }

        [AutocodeGenerated]
        public System.Collections.Generic.List<object> ValidationRules { get; } = new System.Collections.Generic.List<object>();

        [AutocodeGenerated]
        public bool? ValidatesOnExceptions { get; set; }

        [AutocodeGenerated]
        public bool? ValidatesOnDataErrors { get; set; }

        [AutocodeGenerated]
        public bool? ValidatesOnNotifyDataErrors { get; set; }

        [AutocodeGenerated]
        public string StringFormat { get; set; }

        [AutocodeGenerated]
        public string BindingGroupName { get; set; }

        [AutocodeGenerated]
        public object TargetNullValue { get; set; }

        [AutocodeGenerated]
        public bool? BindsDirectlyToSource { get; set; }

        [AutocodeGenerated]
        public bool? IsAsync { get; set; }

        [AutocodeGenerated]
        public string XPath { get; set; }

        [AutocodeGenerated]
        public object From { get; set; }

        [AutocodeGenerated]
        public string Path { get; set; }

    }

    partial class AmmyMultiBind
    {
        [AutocodeGenerated]
        public AmmyMultiBind WithBindingGroupName(string bindingGroupName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            BindingGroupName = bindingGroupName; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithBindings(System.Collections.Generic.List<object> bindings)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            Bindings = bindings; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithConverter(object converter)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            Converter = converter; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithConverterCulture(object converterCulture)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            ConverterCulture = converterCulture; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithConverterCultureFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:146
            ConverterCulture = new AmmyDynamicResource(dynamicResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithConverterCultureFromElementName(string elementName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:156
            ConverterCulture = new ElementNameBindingSource(elementName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithConverterCultureFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:135
            ConverterCulture = new AmmyStaticResource(staticResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithConverterCultureFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:125
            ConverterCulture = new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithConverterFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:146
            Converter = new AmmyDynamicResource(dynamicResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithConverterFromElementName(string elementName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:156
            Converter = new ElementNameBindingSource(elementName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithConverterFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:135
            Converter = new AmmyStaticResource(staticResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithConverterFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:125
            Converter = new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithConverterParameter(object converterParameter)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            ConverterParameter = converterParameter; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithConverterParameterFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:146
            ConverterParameter = new AmmyDynamicResource(dynamicResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithConverterParameterFromElementName(string elementName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:156
            ConverterParameter = new ElementNameBindingSource(elementName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithConverterParameterFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:135
            ConverterParameter = new AmmyStaticResource(staticResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithConverterParameterFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:125
            ConverterParameter = new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithMode(iSukces.Code.Compatibility.System.Windows.Data.XBindingMode? mode)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            Mode = mode; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithNotifyOnSourceUpdated(bool? notifyOnSourceUpdated)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            NotifyOnSourceUpdated = notifyOnSourceUpdated; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithNotifyOnTargetUpdated(bool? notifyOnTargetUpdated)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            NotifyOnTargetUpdated = notifyOnTargetUpdated; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithNotifyOnValidationError(bool? notifyOnValidationError)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            NotifyOnValidationError = notifyOnValidationError; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithStringFormat(string stringFormat)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            StringFormat = stringFormat; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithTargetNullValue(object targetNullValue)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            TargetNullValue = targetNullValue; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithTargetNullValueFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:146
            TargetNullValue = new AmmyDynamicResource(dynamicResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithTargetNullValueFromElementName(string elementName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:156
            TargetNullValue = new ElementNameBindingSource(elementName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithTargetNullValueFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:135
            TargetNullValue = new AmmyStaticResource(staticResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithTargetNullValueFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:125
            TargetNullValue = new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithUpdateSourceTrigger(iSukces.Code.Compatibility.System.Windows.Data.XUpdateSourceTrigger? updateSourceTrigger)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            UpdateSourceTrigger = updateSourceTrigger; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithValidatesOnDataErrors(bool? validatesOnDataErrors)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            ValidatesOnDataErrors = validatesOnDataErrors; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithValidatesOnExceptions(bool? validatesOnExceptions)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            ValidatesOnExceptions = validatesOnExceptions; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBind WithValidatesOnNotifyDataErrors(bool? validatesOnNotifyDataErrors)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            ValidatesOnNotifyDataErrors = validatesOnNotifyDataErrors; return this;
        }

        [AutocodeGenerated]
        public iSukces.Code.Compatibility.System.Windows.Data.XBindingMode? Mode { get; set; }

        [AutocodeGenerated]
        public object Converter { get; set; }

        [AutocodeGenerated]
        public object ConverterCulture { get; set; }

        [AutocodeGenerated]
        public object ConverterParameter { get; set; }

        [AutocodeGenerated]
        public iSukces.Code.Compatibility.System.Windows.Data.XUpdateSourceTrigger? UpdateSourceTrigger { get; set; }

        [AutocodeGenerated]
        public bool? NotifyOnSourceUpdated { get; set; }

        [AutocodeGenerated]
        public bool? NotifyOnTargetUpdated { get; set; }

        [AutocodeGenerated]
        public bool? NotifyOnValidationError { get; set; }

        [AutocodeGenerated]
        public System.Collections.Generic.List<object> ValidationRules { get; } = new System.Collections.Generic.List<object>();

        [AutocodeGenerated]
        public bool? ValidatesOnExceptions { get; set; }

        [AutocodeGenerated]
        public bool? ValidatesOnDataErrors { get; set; }

        [AutocodeGenerated]
        public bool? ValidatesOnNotifyDataErrors { get; set; }

        [AutocodeGenerated]
        public string StringFormat { get; set; }

        [AutocodeGenerated]
        public string BindingGroupName { get; set; }

        [AutocodeGenerated]
        public object TargetNullValue { get; set; }

        [AutocodeGenerated]
        public System.Collections.Generic.List<object> Bindings { get; set; }

    }

    partial class AmmyMultiBindingBuilder
    {
        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithBindingGroupName(string bindingGroupName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            BindingGroupName = bindingGroupName; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithBindings(System.Collections.Generic.List<object> bindings)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            Bindings = bindings; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithConverter(object converter)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            Converter = converter; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithConverterCulture(object converterCulture)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            ConverterCulture = converterCulture; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithConverterCultureFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:146
            ConverterCulture = new AmmyDynamicResource(dynamicResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithConverterCultureFromElementName(string elementName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:156
            ConverterCulture = new ElementNameBindingSource(elementName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithConverterCultureFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:135
            ConverterCulture = new AmmyStaticResource(staticResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithConverterCultureFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:125
            ConverterCulture = new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithConverterFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:146
            Converter = new AmmyDynamicResource(dynamicResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithConverterFromElementName(string elementName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:156
            Converter = new ElementNameBindingSource(elementName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithConverterFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:135
            Converter = new AmmyStaticResource(staticResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithConverterFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:125
            Converter = new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithConverterParameter(object converterParameter)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            ConverterParameter = converterParameter; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithConverterParameterFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:146
            ConverterParameter = new AmmyDynamicResource(dynamicResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithConverterParameterFromElementName(string elementName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:156
            ConverterParameter = new ElementNameBindingSource(elementName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithConverterParameterFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:135
            ConverterParameter = new AmmyStaticResource(staticResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithConverterParameterFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:125
            ConverterParameter = new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithMode(iSukces.Code.Compatibility.System.Windows.Data.XBindingMode? mode)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            Mode = mode; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithNotifyOnSourceUpdated(bool? notifyOnSourceUpdated)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            NotifyOnSourceUpdated = notifyOnSourceUpdated; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithNotifyOnTargetUpdated(bool? notifyOnTargetUpdated)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            NotifyOnTargetUpdated = notifyOnTargetUpdated; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithNotifyOnValidationError(bool? notifyOnValidationError)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            NotifyOnValidationError = notifyOnValidationError; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithStringFormat(string stringFormat)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            StringFormat = stringFormat; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithTargetNullValue(object targetNullValue)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            TargetNullValue = targetNullValue; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithTargetNullValueFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:146
            TargetNullValue = new AmmyDynamicResource(dynamicResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithTargetNullValueFromElementName(string elementName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:156
            TargetNullValue = new ElementNameBindingSource(elementName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithTargetNullValueFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:135
            TargetNullValue = new AmmyStaticResource(staticResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithTargetNullValueFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:125
            TargetNullValue = new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName); return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithUpdateSourceTrigger(iSukces.Code.Compatibility.System.Windows.Data.XUpdateSourceTrigger? updateSourceTrigger)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            UpdateSourceTrigger = updateSourceTrigger; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithValidatesOnDataErrors(bool? validatesOnDataErrors)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            ValidatesOnDataErrors = validatesOnDataErrors; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithValidatesOnExceptions(bool? validatesOnExceptions)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            ValidatesOnExceptions = validatesOnExceptions; return this;
        }

        [AutocodeGenerated]
        public AmmyMultiBindingBuilder WithValidatesOnNotifyDataErrors(bool? validatesOnNotifyDataErrors)
        {
            // generator : FluentBindGenerator.AddFluentMethod:220
            ValidatesOnNotifyDataErrors = validatesOnNotifyDataErrors; return this;
        }

        [AutocodeGenerated]
        private void SetupAmmyBind(AmmyMultiBind bind)
        {
            // generator : FluentBindGenerator.BuildPropertiesAndFluentMethods:250
            if (Mode != null)
                bind.WithMode(Mode);
            if (Converter != null)
                bind.WithConverter(Converter);
            if (ConverterCulture != null)
                bind.WithConverterCulture(ConverterCulture);
            if (ConverterParameter != null)
                bind.WithConverterParameter(ConverterParameter);
            if (UpdateSourceTrigger != null)
                bind.WithUpdateSourceTrigger(UpdateSourceTrigger);
            if (NotifyOnSourceUpdated != null)
                bind.WithNotifyOnSourceUpdated(NotifyOnSourceUpdated);
            if (NotifyOnTargetUpdated != null)
                bind.WithNotifyOnTargetUpdated(NotifyOnTargetUpdated);
            if (NotifyOnValidationError != null)
                bind.WithNotifyOnValidationError(NotifyOnValidationError);
            if (ValidatesOnExceptions != null)
                bind.WithValidatesOnExceptions(ValidatesOnExceptions);
            if (ValidatesOnDataErrors != null)
                bind.WithValidatesOnDataErrors(ValidatesOnDataErrors);
            if (ValidatesOnNotifyDataErrors != null)
                bind.WithValidatesOnNotifyDataErrors(ValidatesOnNotifyDataErrors);
            if (StringFormat != null)
                bind.WithStringFormat(StringFormat);
            if (BindingGroupName != null)
                bind.WithBindingGroupName(BindingGroupName);
            if (TargetNullValue != null)
                bind.WithTargetNullValue(TargetNullValue);
            if (Bindings != null)
                bind.WithBindings(Bindings);
        }

        [AutocodeGenerated]
        public iSukces.Code.Compatibility.System.Windows.Data.XBindingMode? Mode { get; set; }

        [AutocodeGenerated]
        public object ConverterCulture { get; set; }

        [AutocodeGenerated]
        public object ConverterParameter { get; set; }

        [AutocodeGenerated]
        public iSukces.Code.Compatibility.System.Windows.Data.XUpdateSourceTrigger? UpdateSourceTrigger { get; set; }

        [AutocodeGenerated]
        public bool? NotifyOnSourceUpdated { get; set; }

        [AutocodeGenerated]
        public bool? NotifyOnTargetUpdated { get; set; }

        [AutocodeGenerated]
        public bool? NotifyOnValidationError { get; set; }

        [AutocodeGenerated]
        public System.Collections.Generic.List<object> ValidationRules { get; } = new System.Collections.Generic.List<object>();

        [AutocodeGenerated]
        public bool? ValidatesOnExceptions { get; set; }

        [AutocodeGenerated]
        public bool? ValidatesOnDataErrors { get; set; }

        [AutocodeGenerated]
        public bool? ValidatesOnNotifyDataErrors { get; set; }

        [AutocodeGenerated]
        public string StringFormat { get; set; }

        [AutocodeGenerated]
        public string BindingGroupName { get; set; }

        [AutocodeGenerated]
        public object TargetNullValue { get; set; }

        [AutocodeGenerated]
        public System.Collections.Generic.List<object> Bindings { get; set; }

    }

    partial class AmmyObjectBuilder<TPropertyBrowser>
    {
        [AutocodeGenerated]
        public AmmyObjectBuilder<TPropertyBrowser> WithProperty<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, object value)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:31
            var name = CodeUtils.GetMemberPath(func);
            this.WithProperty(name, value);
            return this;
        }

        [AutocodeGenerated]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyAncestorBind<TAncestor, TValue>(Expression<Func<TPropertyBrowser, TValue>> propertyNameExpression, Expression<Func<TAncestor, TValue>> bindToPathExpression, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:102
            var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), bindingSettings);
        }

        [AutocodeGenerated]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyAncestorBind<TAncestor, TValue>(Expression<Func<TPropertyBrowser, TValue>> propertyNameExpression, Expression<Func<TAncestor, TValue>> bindToPathExpression, iSukces.Code.Compatibility.System.Windows.Data.XBindingMode mode, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:115
            var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), mode, bindingSettings);
        }

        [AutocodeGenerated]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyAncestorBind<TAncestor>(Expression<Func<TPropertyBrowser, object>> propertyNameExpression, Expression<Func<TAncestor, object>> bindToPathExpression, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:74
            var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), bindingSettings);
        }

        [AutocodeGenerated]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyAncestorBind<TAncestor>(Expression<Func<TPropertyBrowser, object>> propertyNameExpression, Expression<Func<TAncestor, object>> bindToPathExpression, iSukces.Code.Compatibility.System.Windows.Data.XBindingMode mode, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:88
            var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), mode, bindingSettings);
        }

        [AutocodeGenerated]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyGeneric<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, TValue value)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:41
            var name = CodeUtils.GetMemberPath(func);
            this.WithProperty(name, value);
            return this;
        }

        [AutocodeGenerated]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyGenericNotNull<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, TValue value)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:63
            var name = CodeUtils.GetMemberPath(func);
            return this.WithPropertyNotNull(name, value);
        }

        [AutocodeGenerated]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyNotNull<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, object value)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:53
            var name = CodeUtils.GetMemberPath(func);
            return this.WithPropertyNotNull(name, value);
        }

        [AutocodeGenerated]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertySelfBind(string propertyName, string bindToPath, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:130
            var b          = new AmmyBind(bindToPath).WithBindFromSelf();
            bindingSettings?.Invoke(b);
            return this.WithProperty(propertyName, b);
        }

        [AutocodeGenerated]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertySelfBind<TSelf>(string propertyName, Expression<Func<TSelf, object>> bindToPathExpression, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:144
            var bindToPath = ExpressionTools.GetBindingPath(bindToPathExpression);
            return WithPropertySelfBind(propertyName, bindToPath, bindingSettings);
        }

        [AutocodeGenerated]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertySelfBind<TSelf>(Expression<Func<TPropertyBrowser, object>> propertyNameExpression, Expression<Func<TSelf, object>> bindToPathExpression, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:157
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            var bindToPath = ExpressionTools.GetBindingPath(bindToPathExpression);
            return WithPropertySelfBind(propertyName, bindToPath, bindingSettings);
        }

        [AutocodeGenerated]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyStaticResource([NotNull] Expression<Func<TPropertyBrowser, object>> propertyNameExpression, [NotNull] string resourceName)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:172
            return this.WithProperty(propertyNameExpression, new AmmyStaticResource(resourceName));
        }

        [AutocodeGenerated]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyStaticResource([NotNull] string propertyName, [NotNull] string resourceName)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:183
            (this as IAmmyPropertyContainer).Properties[propertyName] = new AmmyStaticResource(resourceName);
            return this;
        }

    }

    partial class MixinBuilder<TPropertyBrowser>
    {
        [AutocodeGenerated]
        public MixinBuilder<TPropertyBrowser> WithProperty<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, object value)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:31
            var name = CodeUtils.GetMemberPath(func);
            this.WithProperty(name, value);
            return this;
        }

        [AutocodeGenerated]
        public MixinBuilder<TPropertyBrowser> WithPropertyAncestorBind<TAncestor, TValue>(Expression<Func<TPropertyBrowser, TValue>> propertyNameExpression, Expression<Func<TAncestor, TValue>> bindToPathExpression, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:102
            var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), bindingSettings);
        }

        [AutocodeGenerated]
        public MixinBuilder<TPropertyBrowser> WithPropertyAncestorBind<TAncestor, TValue>(Expression<Func<TPropertyBrowser, TValue>> propertyNameExpression, Expression<Func<TAncestor, TValue>> bindToPathExpression, iSukces.Code.Compatibility.System.Windows.Data.XBindingMode mode, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:115
            var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), mode, bindingSettings);
        }

        [AutocodeGenerated]
        public MixinBuilder<TPropertyBrowser> WithPropertyAncestorBind<TAncestor>(Expression<Func<TPropertyBrowser, object>> propertyNameExpression, Expression<Func<TAncestor, object>> bindToPathExpression, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:74
            var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), bindingSettings);
        }

        [AutocodeGenerated]
        public MixinBuilder<TPropertyBrowser> WithPropertyAncestorBind<TAncestor>(Expression<Func<TPropertyBrowser, object>> propertyNameExpression, Expression<Func<TAncestor, object>> bindToPathExpression, iSukces.Code.Compatibility.System.Windows.Data.XBindingMode mode, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:88
            var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), mode, bindingSettings);
        }

        [AutocodeGenerated]
        public MixinBuilder<TPropertyBrowser> WithPropertyGeneric<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, TValue value)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:41
            var name = CodeUtils.GetMemberPath(func);
            this.WithProperty(name, value);
            return this;
        }

        [AutocodeGenerated]
        public MixinBuilder<TPropertyBrowser> WithPropertyGenericNotNull<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, TValue value)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:63
            var name = CodeUtils.GetMemberPath(func);
            return this.WithPropertyNotNull(name, value);
        }

        [AutocodeGenerated]
        public MixinBuilder<TPropertyBrowser> WithPropertyNotNull<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, object value)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:53
            var name = CodeUtils.GetMemberPath(func);
            return this.WithPropertyNotNull(name, value);
        }

        [AutocodeGenerated]
        public MixinBuilder<TPropertyBrowser> WithPropertySelfBind(string propertyName, string bindToPath, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:130
            var b          = new AmmyBind(bindToPath).WithBindFromSelf();
            bindingSettings?.Invoke(b);
            return this.WithProperty(propertyName, b);
        }

        [AutocodeGenerated]
        public MixinBuilder<TPropertyBrowser> WithPropertySelfBind<TSelf>(string propertyName, Expression<Func<TSelf, object>> bindToPathExpression, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:144
            var bindToPath = ExpressionTools.GetBindingPath(bindToPathExpression);
            return WithPropertySelfBind(propertyName, bindToPath, bindingSettings);
        }

        [AutocodeGenerated]
        public MixinBuilder<TPropertyBrowser> WithPropertySelfBind<TSelf>(Expression<Func<TPropertyBrowser, object>> propertyNameExpression, Expression<Func<TSelf, object>> bindToPathExpression, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:157
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            var bindToPath = ExpressionTools.GetBindingPath(bindToPathExpression);
            return WithPropertySelfBind(propertyName, bindToPath, bindingSettings);
        }

        [AutocodeGenerated]
        public MixinBuilder<TPropertyBrowser> WithPropertyStaticResource([NotNull] Expression<Func<TPropertyBrowser, object>> propertyNameExpression, [NotNull] string resourceName)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:172
            return this.WithProperty(propertyNameExpression, new AmmyStaticResource(resourceName));
        }

        [AutocodeGenerated]
        public MixinBuilder<TPropertyBrowser> WithPropertyStaticResource([NotNull] string propertyName, [NotNull] string resourceName)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:183
            (this as IAmmyPropertyContainer).Properties[propertyName] = new AmmyStaticResource(resourceName);
            return this;
        }

    }
}
namespace iSukces.Code.Compatibility.System.Windows.Data
{
    partial class XMultiBinding
    {
        [AutocodeGenerated]
        public XBindingMode? Mode { get; set; }

        [AutocodeGenerated]
        public object Converter { get; set; }

        [AutocodeGenerated]
        public object ConverterCulture { get; set; }

        [AutocodeGenerated]
        public object ConverterParameter { get; set; }

        [AutocodeGenerated]
        public XUpdateSourceTrigger? UpdateSourceTrigger { get; set; }

        [AutocodeGenerated]
        public bool? NotifyOnSourceUpdated { get; set; }

        [AutocodeGenerated]
        public bool? NotifyOnTargetUpdated { get; set; }

        [AutocodeGenerated]
        public bool? NotifyOnValidationError { get; set; }

        [AutocodeGenerated]
        public List<object> ValidationRules { get; } = new List<object>();

        [AutocodeGenerated]
        public bool? ValidatesOnExceptions { get; set; }

        [AutocodeGenerated]
        public bool? ValidatesOnDataErrors { get; set; }

        [AutocodeGenerated]
        public bool? ValidatesOnNotifyDataErrors { get; set; }

        [AutocodeGenerated]
        public string StringFormat { get; set; }

        [AutocodeGenerated]
        public string BindingGroupName { get; set; }

        [AutocodeGenerated]
        public object TargetNullValue { get; set; }

        [AutocodeGenerated]
        public List<object> Bindings { get; set; }

    }
}
namespace iSukces.Code.Interfaces.Ammy
{
    partial class AmmyBuilderExtender<TBuilder,TPropertyBrowser>
    {
        [AutocodeGenerated]
        public AmmyBuilderExtender<TBuilder,TPropertyBrowser> WithProperty<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, object value)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:31
            var name = CodeUtils.GetMemberPath(func);
            this.WithProperty(name, value);
            return this;
        }

        [AutocodeGenerated]
        public AmmyBuilderExtender<TBuilder,TPropertyBrowser> WithPropertyAncestorBind<TAncestor, TValue>(Expression<Func<TPropertyBrowser, TValue>> propertyNameExpression, Expression<Func<TAncestor, TValue>> bindToPathExpression, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:102
            var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), bindingSettings);
        }

        [AutocodeGenerated]
        public AmmyBuilderExtender<TBuilder,TPropertyBrowser> WithPropertyAncestorBind<TAncestor, TValue>(Expression<Func<TPropertyBrowser, TValue>> propertyNameExpression, Expression<Func<TAncestor, TValue>> bindToPathExpression, iSukces.Code.Compatibility.System.Windows.Data.XBindingMode mode, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:115
            var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), mode, bindingSettings);
        }

        [AutocodeGenerated]
        public AmmyBuilderExtender<TBuilder,TPropertyBrowser> WithPropertyAncestorBind<TAncestor>(Expression<Func<TPropertyBrowser, object>> propertyNameExpression, Expression<Func<TAncestor, object>> bindToPathExpression, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:74
            var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), bindingSettings);
        }

        [AutocodeGenerated]
        public AmmyBuilderExtender<TBuilder,TPropertyBrowser> WithPropertyAncestorBind<TAncestor>(Expression<Func<TPropertyBrowser, object>> propertyNameExpression, Expression<Func<TAncestor, object>> bindToPathExpression, iSukces.Code.Compatibility.System.Windows.Data.XBindingMode mode, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:88
            var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), mode, bindingSettings);
        }

        [AutocodeGenerated]
        public AmmyBuilderExtender<TBuilder,TPropertyBrowser> WithPropertyGeneric<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, TValue value)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:41
            var name = CodeUtils.GetMemberPath(func);
            this.WithProperty(name, value);
            return this;
        }

        [AutocodeGenerated]
        public AmmyBuilderExtender<TBuilder,TPropertyBrowser> WithPropertyGenericNotNull<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, TValue value)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:63
            var name = CodeUtils.GetMemberPath(func);
            return this.WithPropertyNotNull(name, value);
        }

        [AutocodeGenerated]
        public AmmyBuilderExtender<TBuilder,TPropertyBrowser> WithPropertyNotNull<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, object value)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:53
            var name = CodeUtils.GetMemberPath(func);
            return this.WithPropertyNotNull(name, value);
        }

        [AutocodeGenerated]
        public AmmyBuilderExtender<TBuilder,TPropertyBrowser> WithPropertySelfBind(string propertyName, string bindToPath, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:130
            var b          = new AmmyBind(bindToPath).WithBindFromSelf();
            bindingSettings?.Invoke(b);
            return this.WithProperty(propertyName, b);
        }

        [AutocodeGenerated]
        public AmmyBuilderExtender<TBuilder,TPropertyBrowser> WithPropertySelfBind<TSelf>(string propertyName, Expression<Func<TSelf, object>> bindToPathExpression, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:144
            var bindToPath = ExpressionTools.GetBindingPath(bindToPathExpression);
            return WithPropertySelfBind(propertyName, bindToPath, bindingSettings);
        }

        [AutocodeGenerated]
        public AmmyBuilderExtender<TBuilder,TPropertyBrowser> WithPropertySelfBind<TSelf>(Expression<Func<TPropertyBrowser, object>> propertyNameExpression, Expression<Func<TSelf, object>> bindToPathExpression, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:157
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            var bindToPath = ExpressionTools.GetBindingPath(bindToPathExpression);
            return WithPropertySelfBind(propertyName, bindToPath, bindingSettings);
        }

        [AutocodeGenerated]
        public AmmyBuilderExtender<TBuilder,TPropertyBrowser> WithPropertyStaticResource([NotNull] Expression<Func<TPropertyBrowser, object>> propertyNameExpression, [NotNull] string resourceName)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:172
            return this.WithProperty(propertyNameExpression, new AmmyStaticResource(resourceName));
        }

        [AutocodeGenerated]
        public AmmyBuilderExtender<TBuilder,TPropertyBrowser> WithPropertyStaticResource([NotNull] string propertyName, [NotNull] string resourceName)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:183
            (this as IAmmyPropertyContainer).Properties[propertyName] = new AmmyStaticResource(resourceName);
            return this;
        }

    }
}
