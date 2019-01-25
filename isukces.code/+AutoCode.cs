using isukces.code;
using isukces.code.Ammy;
using isukces.code.interfaces.Ammy;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

// ReSharper disable once CheckNamespace
namespace isukces.code.Ammy
{
    partial class MixinBuilder<TPropertyBrowser>
    {
        [AutocodeGeneratedAttribute("AmmyPropertyContainerMethodGenerator")]
        public MixinBuilder<TPropertyBrowser> WithProperty<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, object value)
        {
            // generator : AmmyPropertyContainerMethodGenerator G2
            var mi = AmmyHelper.GetMemberInfo(func);
            this.WithProperty(mi.Member.Name, value);
            return this;
        }

        [AutocodeGeneratedAttribute("AmmyPropertyContainerMethodGenerator")]
        public MixinBuilder<TPropertyBrowser> WithPropertyAncestorBind<TAncestor, TValue>(Expression<Func<TPropertyBrowser, TValue>> propertyNameExpression, Expression<Func<TAncestor, TValue>> bindToPathExpression, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator G9
            var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), bindingSettings);
        }

        [AutocodeGeneratedAttribute("AmmyPropertyContainerMethodGenerator")]
        public MixinBuilder<TPropertyBrowser> WithPropertyAncestorBind<TAncestor, TValue>(Expression<Func<TPropertyBrowser, TValue>> propertyNameExpression, Expression<Func<TAncestor, TValue>> bindToPathExpression, DataBindingMode mode, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator G9 ver2
            var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), mode, bindingSettings);
        }

        [AutocodeGeneratedAttribute("AmmyPropertyContainerMethodGenerator")]
        public MixinBuilder<TPropertyBrowser> WithPropertyAncestorBind<TAncestor>(Expression<Func<TPropertyBrowser, object>> propertyNameExpression, Expression<Func<TAncestor, object>> bindToPathExpression, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator G6
            var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), bindingSettings);
        }

        [AutocodeGeneratedAttribute("AmmyPropertyContainerMethodGenerator")]
        public MixinBuilder<TPropertyBrowser> WithPropertyAncestorBind<TAncestor>(Expression<Func<TPropertyBrowser, object>> propertyNameExpression, Expression<Func<TAncestor, object>> bindToPathExpression, DataBindingMode mode, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator G6 ver2
            var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), mode, bindingSettings);
        }

        [AutocodeGeneratedAttribute("AmmyPropertyContainerMethodGenerator")]
        public MixinBuilder<TPropertyBrowser> WithPropertyGeneric<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, TValue value)
        {
            // generator : AmmyPropertyContainerMethodGenerator G3
            var mi = AmmyHelper.GetMemberInfo(func);
            this.WithProperty(mi.Member.Name, value);
            return this;
        }

        [AutocodeGeneratedAttribute("AmmyPropertyContainerMethodGenerator")]
        public MixinBuilder<TPropertyBrowser> WithPropertyGenericNotNull<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, TValue value)
        {
            // generator : AmmyPropertyContainerMethodGenerator G5
            var mi = AmmyHelper.GetMemberInfo(func);
            return this.WithPropertyNotNull(mi.Member.Name, value);
        }

        [AutocodeGeneratedAttribute("AmmyPropertyContainerMethodGenerator")]
        public MixinBuilder<TPropertyBrowser> WithPropertyNotNull<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, object value)
        {
            // generator : AmmyPropertyContainerMethodGenerator G4
            var mi = AmmyHelper.GetMemberInfo(func);
            return this.WithPropertyNotNull(mi.Member.Name, value);
        }

        [AutocodeGeneratedAttribute("AmmyPropertyContainerMethodGenerator")]
        public MixinBuilder<TPropertyBrowser> WithPropertyStaticResource([NotNull] Expression<Func<TPropertyBrowser, object>> propertyNameExpression, [NotNull] string resourceName)
        {
            // generator : AmmyPropertyContainerMethodGenerator G7
            return this.WithProperty(propertyNameExpression, new AmmyStaticResource(resourceName));
        }

        [AutocodeGeneratedAttribute("AmmyPropertyContainerMethodGenerator")]
        public MixinBuilder<TPropertyBrowser> WithPropertyStaticResource([NotNull] string propertyName, [NotNull] string resourceName)
        {
            // generator : AmmyPropertyContainerMethodGenerator G8
            (this as IAmmyPropertyContainer).Properties[propertyName] = new AmmyStaticResource(resourceName);
            return this;
        }

    }

    partial class AmmyObjectBuilder<TPropertyBrowser>
    {
        [AutocodeGeneratedAttribute("AmmyPropertyContainerMethodGenerator")]
        public AmmyObjectBuilder<TPropertyBrowser> WithProperty<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, object value)
        {
            // generator : AmmyPropertyContainerMethodGenerator G2
            var mi = AmmyHelper.GetMemberInfo(func);
            this.WithProperty(mi.Member.Name, value);
            return this;
        }

        [AutocodeGeneratedAttribute("AmmyPropertyContainerMethodGenerator")]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyAncestorBind<TAncestor, TValue>(Expression<Func<TPropertyBrowser, TValue>> propertyNameExpression, Expression<Func<TAncestor, TValue>> bindToPathExpression, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator G9
            var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), bindingSettings);
        }

        [AutocodeGeneratedAttribute("AmmyPropertyContainerMethodGenerator")]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyAncestorBind<TAncestor, TValue>(Expression<Func<TPropertyBrowser, TValue>> propertyNameExpression, Expression<Func<TAncestor, TValue>> bindToPathExpression, DataBindingMode mode, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator G9 ver2
            var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), mode, bindingSettings);
        }

        [AutocodeGeneratedAttribute("AmmyPropertyContainerMethodGenerator")]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyAncestorBind<TAncestor>(Expression<Func<TPropertyBrowser, object>> propertyNameExpression, Expression<Func<TAncestor, object>> bindToPathExpression, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator G6
            var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), bindingSettings);
        }

        [AutocodeGeneratedAttribute("AmmyPropertyContainerMethodGenerator")]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyAncestorBind<TAncestor>(Expression<Func<TPropertyBrowser, object>> propertyNameExpression, Expression<Func<TAncestor, object>> bindToPathExpression, DataBindingMode mode, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            // generator : AmmyPropertyContainerMethodGenerator G6 ver2
            var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), mode, bindingSettings);
        }

        [AutocodeGeneratedAttribute("AmmyPropertyContainerMethodGenerator")]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyGeneric<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, TValue value)
        {
            // generator : AmmyPropertyContainerMethodGenerator G3
            var mi = AmmyHelper.GetMemberInfo(func);
            this.WithProperty(mi.Member.Name, value);
            return this;
        }

        [AutocodeGeneratedAttribute("AmmyPropertyContainerMethodGenerator")]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyGenericNotNull<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, TValue value)
        {
            // generator : AmmyPropertyContainerMethodGenerator G5
            var mi = AmmyHelper.GetMemberInfo(func);
            return this.WithPropertyNotNull(mi.Member.Name, value);
        }

        [AutocodeGeneratedAttribute("AmmyPropertyContainerMethodGenerator")]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyNotNull<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, object value)
        {
            // generator : AmmyPropertyContainerMethodGenerator G4
            var mi = AmmyHelper.GetMemberInfo(func);
            return this.WithPropertyNotNull(mi.Member.Name, value);
        }

        [AutocodeGeneratedAttribute("AmmyPropertyContainerMethodGenerator")]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyStaticResource([NotNull] Expression<Func<TPropertyBrowser, object>> propertyNameExpression, [NotNull] string resourceName)
        {
            // generator : AmmyPropertyContainerMethodGenerator G7
            return this.WithProperty(propertyNameExpression, new AmmyStaticResource(resourceName));
        }

        [AutocodeGeneratedAttribute("AmmyPropertyContainerMethodGenerator")]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyStaticResource([NotNull] string propertyName, [NotNull] string resourceName)
        {
            // generator : AmmyPropertyContainerMethodGenerator G8
            (this as IAmmyPropertyContainer).Properties[propertyName] = new AmmyStaticResource(resourceName);
            return this;
        }

    }

    partial class AmmyBind
    {
        [AutocodeGenerated]
        public AmmyBind WithBindFrom(object from)
        {
            // generator : FluentBindGenerator G1
            From = from; return this;
        }

        [AutocodeGenerated]
        public AmmyBind WithBindFromAncestor(Type ancestorType, int? level = null)
        {
            // generator : FluentBindGenerator G4
            From = new AncestorBindingSource(ancestorType, level); return this;
        }

        [AutocodeGenerated]
        public AmmyBind WithBindFromAncestor<TAncestor>(int? level = null)
        {
            // generator : FluentBindGenerator G5
            From = new AncestorBindingSource(typeof(TAncestor),  level); return this;
        }

        [AutocodeGenerated]
        public AmmyBind WithBindFromResource(string resourceName)
        {
            // generator : FluentBindGenerator G3
            From = new AmmyStaticResource(resourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBind WithBindFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator G2
            From = new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName); return this;
        }

        [AutocodeGenerated]
        public AmmyBind WithBindingGroupName(string bindingGroupName)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("BindingGroupName", bindingGroupName);
        }

        [AutocodeGenerated]
        public AmmyBind WithBindingGroupName(object bindingGroupName)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("BindingGroupName", bindingGroupName);
        }

        [AutocodeGenerated]
        public AmmyBind WithBindsDirectlyToSource(bool bindsDirectlyToSource)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("BindsDirectlyToSource", bindsDirectlyToSource);
        }

        [AutocodeGenerated]
        public AmmyBind WithBindsDirectlyToSource(object bindsDirectlyToSource)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("BindsDirectlyToSource", bindsDirectlyToSource);
        }

        [AutocodeGenerated]
        public AmmyBind WithConverter(object converter)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("Converter", converter);
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterCulture(object converterCulture)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("ConverterCulture", converterCulture);
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterFromResource(string resourceName)
        {
            // generator : FluentBindGenerator G3
            return WithSetParameter("Converter", new AmmyStaticResource(resourceName));
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator G2
            return WithSetParameter("Converter", new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName));
        }

        [AutocodeGenerated]
        public AmmyBind WithConverterParameter(object converterParameter)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("ConverterParameter", converterParameter);
        }

        [AutocodeGenerated]
        public AmmyBind WithIsAsync(bool isAsync)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("IsAsync", isAsync);
        }

        [AutocodeGenerated]
        public AmmyBind WithIsAsync(object isAsync)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("IsAsync", isAsync);
        }

        [AutocodeGenerated]
        public AmmyBind WithMode(DataBindingMode mode)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("Mode", mode);
        }

        [AutocodeGenerated]
        public AmmyBind WithMode(object mode)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("Mode", mode);
        }

        [AutocodeGenerated]
        public AmmyBind WithNotifyOnSourceUpdated(bool notifyOnSourceUpdated)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("NotifyOnSourceUpdated", notifyOnSourceUpdated);
        }

        [AutocodeGenerated]
        public AmmyBind WithNotifyOnSourceUpdated(object notifyOnSourceUpdated)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("NotifyOnSourceUpdated", notifyOnSourceUpdated);
        }

        [AutocodeGenerated]
        public AmmyBind WithNotifyOnTargetUpdated(bool notifyOnTargetUpdated)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("NotifyOnTargetUpdated", notifyOnTargetUpdated);
        }

        [AutocodeGenerated]
        public AmmyBind WithNotifyOnTargetUpdated(object notifyOnTargetUpdated)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("NotifyOnTargetUpdated", notifyOnTargetUpdated);
        }

        [AutocodeGenerated]
        public AmmyBind WithNotifyOnValidationError(bool notifyOnValidationError)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("NotifyOnValidationError", notifyOnValidationError);
        }

        [AutocodeGenerated]
        public AmmyBind WithNotifyOnValidationError(object notifyOnValidationError)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("NotifyOnValidationError", notifyOnValidationError);
        }

        [AutocodeGenerated]
        public AmmyBind WithPath(string path)
        {
            // generator : FluentBindGenerator G1
            Path = path; return this;
        }

        [AutocodeGenerated]
        public AmmyBind WithStringFormat(string stringFormat)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("StringFormat", stringFormat);
        }

        [AutocodeGenerated]
        public AmmyBind WithStringFormat(object stringFormat)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("StringFormat", stringFormat);
        }

        [AutocodeGenerated]
        public AmmyBind WithTargetNullValue(object targetNullValue)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("TargetNullValue", targetNullValue);
        }

        [AutocodeGenerated]
        public AmmyBind WithUpdateSourceTrigger(DataUpdateSourceTrigger updateSourceTrigger)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("UpdateSourceTrigger", updateSourceTrigger);
        }

        [AutocodeGenerated]
        public AmmyBind WithUpdateSourceTrigger(object updateSourceTrigger)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("UpdateSourceTrigger", updateSourceTrigger);
        }

        [AutocodeGenerated]
        public AmmyBind WithValidatesOnDataErrors(bool validatesOnDataErrors)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("ValidatesOnDataErrors", validatesOnDataErrors);
        }

        [AutocodeGenerated]
        public AmmyBind WithValidatesOnDataErrors(object validatesOnDataErrors)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("ValidatesOnDataErrors", validatesOnDataErrors);
        }

        [AutocodeGenerated]
        public AmmyBind WithValidatesOnExceptions(bool validatesOnExceptions)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("ValidatesOnExceptions", validatesOnExceptions);
        }

        [AutocodeGenerated]
        public AmmyBind WithValidatesOnExceptions(object validatesOnExceptions)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("ValidatesOnExceptions", validatesOnExceptions);
        }

        [AutocodeGenerated]
        public AmmyBind WithValidationRules(object validationRules)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("ValidationRules", validationRules);
        }

        [AutocodeGenerated]
        public AmmyBind WithXPath(string xPath)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("XPath", xPath);
        }

        [AutocodeGenerated]
        public AmmyBind WithXPath(object xPath)
        {
            // generator : FluentBindGenerator G1
            return WithSetParameter("XPath", xPath);
        }

    }

    partial class AmmyBindBuilder
    {
        [AutocodeGenerated]
        public AmmyBindBuilder WithBindFrom(object from)
        {
            // generator : FluentBindGenerator G1
            From = from; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithBindFromAncestor(Type ancestorType, int? level = null)
        {
            // generator : FluentBindGenerator G4
            From = new AncestorBindingSource(ancestorType, level); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithBindFromAncestor<TAncestor>(int? level = null)
        {
            // generator : FluentBindGenerator G5
            From = new AncestorBindingSource(typeof(TAncestor),  level); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithBindFromResource(string resourceName)
        {
            // generator : FluentBindGenerator G3
            From = new AmmyStaticResource(resourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithBindFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator G2
            From = new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithBindingGroupName(string bindingGroupName)
        {
            // generator : FluentBindGenerator G1
            BindingGroupName = bindingGroupName; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithBindsDirectlyToSource(bool? bindsDirectlyToSource)
        {
            // generator : FluentBindGenerator G1
            BindsDirectlyToSource = bindsDirectlyToSource; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverter(object converter)
        {
            // generator : FluentBindGenerator G1
            Converter = converter; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterCulture(object converterCulture)
        {
            // generator : FluentBindGenerator G1
            ConverterCulture = converterCulture; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterFromResource(string resourceName)
        {
            // generator : FluentBindGenerator G3
            Converter = new AmmyStaticResource(resourceName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterFromStatic<TStaticPropertyOwner>(string propertyName)
        {
            // generator : FluentBindGenerator G2
            Converter = new StaticBindingSource(typeof(TStaticPropertyOwner), propertyName); return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithConverterParameter(object converterParameter)
        {
            // generator : FluentBindGenerator G1
            ConverterParameter = converterParameter; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithIsAsync(bool? isAsync)
        {
            // generator : FluentBindGenerator G1
            IsAsync = isAsync; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithMode(DataBindingMode? mode)
        {
            // generator : FluentBindGenerator G1
            Mode = mode; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithNotifyOnSourceUpdated(bool? notifyOnSourceUpdated)
        {
            // generator : FluentBindGenerator G1
            NotifyOnSourceUpdated = notifyOnSourceUpdated; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithNotifyOnTargetUpdated(bool? notifyOnTargetUpdated)
        {
            // generator : FluentBindGenerator G1
            NotifyOnTargetUpdated = notifyOnTargetUpdated; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithNotifyOnValidationError(bool? notifyOnValidationError)
        {
            // generator : FluentBindGenerator G1
            NotifyOnValidationError = notifyOnValidationError; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithPath(string path)
        {
            // generator : FluentBindGenerator G1
            Path = path; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithStringFormat(string stringFormat)
        {
            // generator : FluentBindGenerator G1
            StringFormat = stringFormat; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithTargetNullValue(object targetNullValue)
        {
            // generator : FluentBindGenerator G1
            TargetNullValue = targetNullValue; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithUpdateSourceTrigger(DataUpdateSourceTrigger? updateSourceTrigger)
        {
            // generator : FluentBindGenerator G1
            UpdateSourceTrigger = updateSourceTrigger; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithValidatesOnDataErrors(bool? validatesOnDataErrors)
        {
            // generator : FluentBindGenerator G1
            ValidatesOnDataErrors = validatesOnDataErrors; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithValidatesOnExceptions(bool? validatesOnExceptions)
        {
            // generator : FluentBindGenerator G1
            ValidatesOnExceptions = validatesOnExceptions; return this;
        }

        [AutocodeGenerated]
        public AmmyBindBuilder WithXPath(string xPath)
        {
            // generator : FluentBindGenerator G1
            XPath = xPath; return this;
        }

        [AutocodeGenerated]
        private void SetupAmmyBind(AmmyBind bind)
        {
            // generator : FluentBindGenerator G11
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

        public DataBindingMode? Mode { get; set; }

        public List<object> ValidationRules { get; } = new List<object>();

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

        public DataUpdateSourceTrigger? UpdateSourceTrigger { get; set; }

        public bool? ValidatesOnDataErrors { get; set; }

        public bool? ValidatesOnExceptions { get; set; }

        public string XPath { get; set; }

        public object From { get; set; }

        public string Path { get; set; }

    }
}
