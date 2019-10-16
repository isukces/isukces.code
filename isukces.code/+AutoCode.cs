// ReSharper disable All
using isukces.code;
using isukces.code.Ammy;
using isukces.code.interfaces.Ammy;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace isukces.code.Ammy
{
    partial class AmmyBind
    {
        [AutocodeGenerated]
        public AmmyBind WithBindFrom(object from)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            From = from; return this;
        }

        [AutocodeGenerated]
        public AmmyBind WithBindFromAncestor(System.Type ancestorType, int? level = null)
        {
            // generator : FluentBindGenerator.AddFluentMethod:156
            From = new AncestorBindingSource(ancestorType, level); return this;
        }

        [AutocodeGenerated]
        public AmmyBind WithBindFromAncestor<TAncestor>(int? level = null)
        {
            // generator : FluentBindGenerator.AddFluentMethod:167
            From = new AncestorBindingSource(typeof(TAncestor),  level); return this;
        }

        [AutocodeGenerated]
        public AmmyBind WithBindFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:143
            From = new AmmyDynamicResource(dynamicResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBind WithBindFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:132
            From = new AmmyStaticResource(staticResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBind WithBindFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:122
            From = new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName); return this;
        }

        [AutocodeGenerated]
        public AmmyBind WithBindingGroupName(string bindingGroupName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("BindingGroupName", bindingGroupName);
        }

        [AutocodeGenerated]
        public AmmyBind WithBindingGroupName(object bindingGroupName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("BindingGroupName", bindingGroupName);
        }

        [AutocodeGenerated]
        public AmmyBind WithBindsDirectlyToSource(bool bindsDirectlyToSource)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("BindsDirectlyToSource", bindsDirectlyToSource);
        }

        [AutocodeGenerated]
        public AmmyBind WithBindsDirectlyToSource(object bindsDirectlyToSource)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("BindsDirectlyToSource", bindsDirectlyToSource);
        }

        [AutocodeGenerated]
        public AmmyBind WithConverter(object converter)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("Converter", converter);
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterCulture(object converterCulture)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("ConverterCulture", converterCulture);
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterCultureFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:143
            return WithSetParameter("ConverterCulture", new AmmyDynamicResource(dynamicResourceName));
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterCultureFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:132
            return WithSetParameter("ConverterCulture", new AmmyStaticResource(staticResourceName));
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterCultureFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:122
            return WithSetParameter("ConverterCulture", new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName));
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:143
            return WithSetParameter("Converter", new AmmyDynamicResource(dynamicResourceName));
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:132
            return WithSetParameter("Converter", new AmmyStaticResource(staticResourceName));
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:122
            return WithSetParameter("Converter", new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName));
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterParameter(object converterParameter)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("ConverterParameter", converterParameter);
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterParameterFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:143
            return WithSetParameter("ConverterParameter", new AmmyDynamicResource(dynamicResourceName));
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterParameterFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:132
            return WithSetParameter("ConverterParameter", new AmmyStaticResource(staticResourceName));
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterParameterFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:122
            return WithSetParameter("ConverterParameter", new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName));
        }

        [AutocodeGenerated]
        public AmmyBind WithIsAsync(bool isAsync)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("IsAsync", isAsync);
        }

        [AutocodeGenerated]
        public AmmyBind WithIsAsync(object isAsync)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("IsAsync", isAsync);
        }

        [AutocodeGenerated]
        public AmmyBind WithMode(isukces.code.Compatibility.System.Windows.Data.XBindingMode mode)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("Mode", mode);
        }

        [AutocodeGenerated]
        public AmmyBind WithMode(object mode)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("Mode", mode);
        }

        [AutocodeGenerated]
        public AmmyBind WithNotifyOnSourceUpdated(bool notifyOnSourceUpdated)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("NotifyOnSourceUpdated", notifyOnSourceUpdated);
        }

        [AutocodeGenerated]
        public AmmyBind WithNotifyOnSourceUpdated(object notifyOnSourceUpdated)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("NotifyOnSourceUpdated", notifyOnSourceUpdated);
        }

        [AutocodeGenerated]
        public AmmyBind WithNotifyOnTargetUpdated(bool notifyOnTargetUpdated)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("NotifyOnTargetUpdated", notifyOnTargetUpdated);
        }

        [AutocodeGenerated]
        public AmmyBind WithNotifyOnTargetUpdated(object notifyOnTargetUpdated)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("NotifyOnTargetUpdated", notifyOnTargetUpdated);
        }

        [AutocodeGenerated]
        public AmmyBind WithNotifyOnValidationError(bool notifyOnValidationError)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("NotifyOnValidationError", notifyOnValidationError);
        }

        [AutocodeGenerated]
        public AmmyBind WithNotifyOnValidationError(object notifyOnValidationError)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("NotifyOnValidationError", notifyOnValidationError);
        }

        [AutocodeGenerated]
        public AmmyBind WithPath(string path)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            Path = path; return this;
        }

        [AutocodeGenerated]
        public AmmyBind WithStringFormat(string stringFormat)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("StringFormat", stringFormat);
        }

        [AutocodeGenerated]
        public AmmyBind WithStringFormat(object stringFormat)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("StringFormat", stringFormat);
        }

        [AutocodeGenerated]
        public AmmyBind WithTargetNullValue(object targetNullValue)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("TargetNullValue", targetNullValue);
        }

        [AutocodeGenerated]
        public AmmyBind WithUpdateSourceTrigger(isukces.code.Compatibility.System.Windows.Data.XUpdateSourceTrigger updateSourceTrigger)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("UpdateSourceTrigger", updateSourceTrigger);
        }

        [AutocodeGenerated]
        public AmmyBind WithUpdateSourceTrigger(object updateSourceTrigger)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("UpdateSourceTrigger", updateSourceTrigger);
        }

        [AutocodeGenerated]
        public AmmyBind WithValidatesOnDataErrors(bool validatesOnDataErrors)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("ValidatesOnDataErrors", validatesOnDataErrors);
        }

        [AutocodeGenerated]
        public AmmyBind WithValidatesOnDataErrors(object validatesOnDataErrors)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("ValidatesOnDataErrors", validatesOnDataErrors);
        }

        [AutocodeGenerated]
        public AmmyBind WithValidatesOnExceptions(bool validatesOnExceptions)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("ValidatesOnExceptions", validatesOnExceptions);
        }

        [AutocodeGenerated]
        public AmmyBind WithValidatesOnExceptions(object validatesOnExceptions)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("ValidatesOnExceptions", validatesOnExceptions);
        }

        [AutocodeGenerated]
        public AmmyBind WithValidationRules(object validationRules)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("ValidationRules", validationRules);
        }

        [AutocodeGenerated]
        public AmmyBind WithXPath(string xPath)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("XPath", xPath);
        }

        [AutocodeGenerated]
        public AmmyBind WithXPath(object xPath)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            return WithSetParameter("XPath", xPath);
        }

        [AutocodeGenerated]
        public AmmyBind WithXPathFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:143
            return WithSetParameter("XPath", new AmmyDynamicResource(dynamicResourceName));
        }

        [AutocodeGenerated]
        public AmmyBind WithXPathFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:132
            return WithSetParameter("XPath", new AmmyStaticResource(staticResourceName));
        }

        [AutocodeGenerated]
        public AmmyBind WithXPathFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:122
            return WithSetParameter("XPath", new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName));
        }

    }

    partial class AmmyBindBuilder
    {
        [AutocodeGenerated]
        public AmmyBindBuilder WithBindFrom(object from)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            From = from; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithBindFromAncestor(System.Type ancestorType, int? level = null)
        {
            // generator : FluentBindGenerator.AddFluentMethod:156
            From = new AncestorBindingSource(ancestorType, level); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithBindFromAncestor<TAncestor>(int? level = null)
        {
            // generator : FluentBindGenerator.AddFluentMethod:167
            From = new AncestorBindingSource(typeof(TAncestor),  level); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithBindFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:143
            From = new AmmyDynamicResource(dynamicResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithBindFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:132
            From = new AmmyStaticResource(staticResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithBindFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:122
            From = new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithBindingGroupName(string bindingGroupName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            BindingGroupName = bindingGroupName; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithBindsDirectlyToSource(bool? bindsDirectlyToSource)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            BindsDirectlyToSource = bindsDirectlyToSource; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverter(object converter)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            Converter = converter; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterCulture(object converterCulture)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            ConverterCulture = converterCulture; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterCultureFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:143
            ConverterCulture = new AmmyDynamicResource(dynamicResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterCultureFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:132
            ConverterCulture = new AmmyStaticResource(staticResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterCultureFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:122
            ConverterCulture = new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:143
            Converter = new AmmyDynamicResource(dynamicResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:132
            Converter = new AmmyStaticResource(staticResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:122
            Converter = new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterParameter(object converterParameter)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            ConverterParameter = converterParameter; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterParameterFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:143
            ConverterParameter = new AmmyDynamicResource(dynamicResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterParameterFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:132
            ConverterParameter = new AmmyStaticResource(staticResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterParameterFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:122
            ConverterParameter = new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithIsAsync(bool? isAsync)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            IsAsync = isAsync; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithMode(isukces.code.Compatibility.System.Windows.Data.XBindingMode? mode)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            Mode = mode; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithNotifyOnSourceUpdated(bool? notifyOnSourceUpdated)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            NotifyOnSourceUpdated = notifyOnSourceUpdated; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithNotifyOnTargetUpdated(bool? notifyOnTargetUpdated)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            NotifyOnTargetUpdated = notifyOnTargetUpdated; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithNotifyOnValidationError(bool? notifyOnValidationError)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            NotifyOnValidationError = notifyOnValidationError; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithPath(string path)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            Path = path; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithStringFormat(string stringFormat)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            StringFormat = stringFormat; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithTargetNullValue(object targetNullValue)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            TargetNullValue = targetNullValue; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithTargetNullValueFromDynamicResource(string dynamicResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:143
            TargetNullValue = new AmmyDynamicResource(dynamicResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithTargetNullValueFromResource(string staticResourceName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:132
            TargetNullValue = new AmmyStaticResource(staticResourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithTargetNullValueFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator.AddFluentMethod:122
            TargetNullValue = new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithUpdateSourceTrigger(isukces.code.Compatibility.System.Windows.Data.XUpdateSourceTrigger? updateSourceTrigger)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            UpdateSourceTrigger = updateSourceTrigger; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithValidatesOnDataErrors(bool? validatesOnDataErrors)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            ValidatesOnDataErrors = validatesOnDataErrors; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithValidatesOnExceptions(bool? validatesOnExceptions)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            ValidatesOnExceptions = validatesOnExceptions; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithXPath(string xPath)
        {
            // generator : FluentBindGenerator.AddFluentMethod:179
            XPath = xPath; return this;
        }

        [AutocodeGenerated]
        private void SetupAmmyBind(AmmyBind bind)
        {
            // generator : FluentBindGenerator.BuildAmmyBindBuilder:205
            if (Mode != null)
                bind.WithMode(Mode);
            if (BindingGroupName != null)
                bind.WithBindingGroupName(BindingGroupName);
            if (BindsDirectlyToSource != null)
                bind.WithBindsDirectlyToSource(BindsDirectlyToSource);
            if (Converter != null)
                bind.WithConverter(Converter);
            if (ConverterCulture != null)
                bind.WithConverterCulture(ConverterCulture);
            if (ConverterParameter != null)
                bind.WithConverterParameter(ConverterParameter);
            if (IsAsync != null)
                bind.WithIsAsync(IsAsync);
            if (NotifyOnSourceUpdated != null)
                bind.WithNotifyOnSourceUpdated(NotifyOnSourceUpdated);
            if (NotifyOnTargetUpdated != null)
                bind.WithNotifyOnTargetUpdated(NotifyOnTargetUpdated);
            if (NotifyOnValidationError != null)
                bind.WithNotifyOnValidationError(NotifyOnValidationError);
            if (StringFormat != null)
                bind.WithStringFormat(StringFormat);
            if (TargetNullValue != null)
                bind.WithTargetNullValue(TargetNullValue);
            if (UpdateSourceTrigger != null)
                bind.WithUpdateSourceTrigger(UpdateSourceTrigger);
            if (ValidatesOnDataErrors != null)
                bind.WithValidatesOnDataErrors(ValidatesOnDataErrors);
            if (ValidatesOnExceptions != null)
                bind.WithValidatesOnExceptions(ValidatesOnExceptions);
            if (XPath != null)
                bind.WithXPath(XPath);
            if (From != null)
                bind.WithBindFrom(From);
            if (Path != null)
                bind.WithPath(Path);
        }

        public isukces.code.Compatibility.System.Windows.Data.XBindingMode? Mode { get; set; }

        public System.Collections.Generic.List<object> ValidationRules { get; } = new System.Collections.Generic.List<object>();

        public string BindingGroupName { get; set; }

        public bool? BindsDirectlyToSource { get; set; }

        public object Converter { get; set; }

        public object ConverterCulture { get; set; }

        public object ConverterParameter { get; set; }

        public bool? IsAsync { get; set; }

        public bool? NotifyOnSourceUpdated { get; set; }

        public bool? NotifyOnTargetUpdated { get; set; }

        public bool? NotifyOnValidationError { get; set; }

        public string StringFormat { get; set; }

        public object TargetNullValue { get; set; }

        public isukces.code.Compatibility.System.Windows.Data.XUpdateSourceTrigger? UpdateSourceTrigger { get; set; }

        public bool? ValidatesOnDataErrors { get; set; }

        public bool? ValidatesOnExceptions { get; set; }

        public string XPath { get; set; }

        public object From { get; set; }

        public string Path { get; set; }

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
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyAncestorBind<TAncestor, TValue>(Expression<Func<TPropertyBrowser, TValue>> propertyNameExpression, Expression<Func<TAncestor, TValue>> bindToPathExpression, isukces.code.Compatibility.System.Windows.Data.XBindingMode mode, [CanBeNull] Action<AmmyBind> bindingSettings = null)
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
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyAncestorBind<TAncestor>(Expression<Func<TPropertyBrowser, object>> propertyNameExpression, Expression<Func<TAncestor, object>> bindToPathExpression, isukces.code.Compatibility.System.Windows.Data.XBindingMode mode, [CanBeNull] Action<AmmyBind> bindingSettings = null)
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
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyStaticResource([NotNull] Expression<Func<TPropertyBrowser, object>> propertyNameExpression, [NotNull] string resourceName)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:130
            return this.WithProperty(propertyNameExpression, new AmmyStaticResource(resourceName));
        }

        [AutocodeGenerated]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyStaticResource([NotNull] string propertyName, [NotNull] string resourceName)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:141
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
        public MixinBuilder<TPropertyBrowser> WithPropertyAncestorBind<TAncestor, TValue>(Expression<Func<TPropertyBrowser, TValue>> propertyNameExpression, Expression<Func<TAncestor, TValue>> bindToPathExpression, isukces.code.Compatibility.System.Windows.Data.XBindingMode mode, [CanBeNull] Action<AmmyBind> bindingSettings = null)
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
        public MixinBuilder<TPropertyBrowser> WithPropertyAncestorBind<TAncestor>(Expression<Func<TPropertyBrowser, object>> propertyNameExpression, Expression<Func<TAncestor, object>> bindToPathExpression, isukces.code.Compatibility.System.Windows.Data.XBindingMode mode, [CanBeNull] Action<AmmyBind> bindingSettings = null)
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
        public MixinBuilder<TPropertyBrowser> WithPropertyStaticResource([NotNull] Expression<Func<TPropertyBrowser, object>> propertyNameExpression, [NotNull] string resourceName)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:130
            return this.WithProperty(propertyNameExpression, new AmmyStaticResource(resourceName));
        }

        [AutocodeGenerated]
        public MixinBuilder<TPropertyBrowser> WithPropertyStaticResource([NotNull] string propertyName, [NotNull] string resourceName)
        {
            // generator : AmmyPropertyContainerMethodGenerator.Generate:141
            (this as IAmmyPropertyContainer).Properties[propertyName] = new AmmyStaticResource(resourceName);
            return this;
        }

    }
}
