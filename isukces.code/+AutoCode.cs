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
        [isukces.code.AutocodeGenerated]
        public MixinBuilder<TPropertyBrowser> WithProperty<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, object value)
        {
            var mi = AmmyHelper.GetMemberInfo(func);
            this.WithProperty(mi.Member.Name, value);
            return this;
        }

        [isukces.code.AutocodeGenerated]
        public MixinBuilder<TPropertyBrowser> WithPropertyAncestorBind<TAncestor>(Expression<Func<TPropertyBrowser, object>> propertyNameExpression, Expression<Func<TAncestor, object>> bindToPathExpression, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), bindingSettings);
        }

        [isukces.code.AutocodeGenerated]
        public MixinBuilder<TPropertyBrowser> WithPropertyGeneric<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, TValue value)
        {
            var mi = AmmyHelper.GetMemberInfo(func);
            this.WithProperty(mi.Member.Name, value);
            return this;
        }

        [isukces.code.AutocodeGenerated]
        public MixinBuilder<TPropertyBrowser> WithPropertyGenericNotNull<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, TValue value)
        {
            var mi = AmmyHelper.GetMemberInfo(func);
            return this.WithPropertyNotNull(mi.Member.Name, value);
        }

        [isukces.code.AutocodeGenerated]
        public MixinBuilder<TPropertyBrowser> WithPropertyNotNull<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, object value)
        {
            var mi = AmmyHelper.GetMemberInfo(func);
            return this.WithPropertyNotNull(mi.Member.Name, value);
        }

        [isukces.code.AutocodeGenerated]
        public MixinBuilder<TPropertyBrowser> WithPropertyStaticResource([NotNull] Expression<Func<TPropertyBrowser, object>> propertyNameExpression, [NotNull] string resourceName)
        {
            return this.WithProperty(propertyNameExpression, new AmmyStaticResource(resourceName));
        }

        [isukces.code.AutocodeGenerated]
        public MixinBuilder<TPropertyBrowser> WithPropertyStaticResource([NotNull] string propertyName, [NotNull] string resourceName)
        {
            (this as IAmmyPropertyContainer).Properties[propertyName] = new AmmyStaticResource(resourceName);
            return this;
        }

    }

    partial class AmmyObjectBuilder<TPropertyBrowser>
    {
        [isukces.code.AutocodeGenerated]
        public AmmyObjectBuilder<TPropertyBrowser> WithProperty<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, object value)
        {
            var mi = AmmyHelper.GetMemberInfo(func);
            this.WithProperty(mi.Member.Name, value);
            return this;
        }

        [isukces.code.AutocodeGenerated]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyAncestorBind<TAncestor>(Expression<Func<TPropertyBrowser, object>> propertyNameExpression, Expression<Func<TAncestor, object>> bindToPathExpression, [CanBeNull] Action<AmmyBind> bindingSettings = null)
        {
            var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TAncestor), bindingSettings);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyGeneric<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, TValue value)
        {
            var mi = AmmyHelper.GetMemberInfo(func);
            this.WithProperty(mi.Member.Name, value);
            return this;
        }

        [isukces.code.AutocodeGenerated]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyGenericNotNull<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, TValue value)
        {
            var mi = AmmyHelper.GetMemberInfo(func);
            return this.WithPropertyNotNull(mi.Member.Name, value);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyNotNull<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, object value)
        {
            var mi = AmmyHelper.GetMemberInfo(func);
            return this.WithPropertyNotNull(mi.Member.Name, value);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyStaticResource([NotNull] Expression<Func<TPropertyBrowser, object>> propertyNameExpression, [NotNull] string resourceName)
        {
            return this.WithProperty(propertyNameExpression, new AmmyStaticResource(resourceName));
        }

        [isukces.code.AutocodeGenerated]
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyStaticResource([NotNull] string propertyName, [NotNull] string resourceName)
        {
            (this as IAmmyPropertyContainer).Properties[propertyName] = new AmmyStaticResource(resourceName);
            return this;
        }

    }

    partial class AmmyBind
    {
        [isukces.code.AutocodeGenerated]
        public AmmyBind WithBindFromAncestor<T>(int? level = null)
        {
            return this.WithBindFromAncestor(typeof(T), level);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithBindingGroupName(string bindingGroupName)
        {
            return WithSetParameter("BindingGroupName", bindingGroupName);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithBindingGroupName(object bindingGroupName)
        {
            return WithSetParameter("BindingGroupName", bindingGroupName);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithBindsDirectlyToSource(bool bindsDirectlyToSource)
        {
            return WithSetParameter("BindsDirectlyToSource", bindsDirectlyToSource);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithBindsDirectlyToSource(object bindsDirectlyToSource)
        {
            return WithSetParameter("BindsDirectlyToSource", bindsDirectlyToSource);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithConverter(object converter)
        {
            return WithSetParameter("Converter", converter);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithConverterCulture(object converterCulture)
        {
            return WithSetParameter("ConverterCulture", converterCulture);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithConverterParameter(object converterParameter)
        {
            return WithSetParameter("ConverterParameter", converterParameter);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithConverterStatic<T>(string propertyName)
        {
            return this.WithConverterStatic(typeof(T), propertyName);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithIsAsync(bool isAsync)
        {
            return WithSetParameter("IsAsync", isAsync);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithIsAsync(object isAsync)
        {
            return WithSetParameter("IsAsync", isAsync);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithMode(DataBindingMode mode)
        {
            return WithSetParameter("Mode", mode);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithMode(object mode)
        {
            return WithSetParameter("Mode", mode);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithNotifyOnSourceUpdated(bool notifyOnSourceUpdated)
        {
            return WithSetParameter("NotifyOnSourceUpdated", notifyOnSourceUpdated);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithNotifyOnSourceUpdated(object notifyOnSourceUpdated)
        {
            return WithSetParameter("NotifyOnSourceUpdated", notifyOnSourceUpdated);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithNotifyOnTargetUpdated(bool notifyOnTargetUpdated)
        {
            return WithSetParameter("NotifyOnTargetUpdated", notifyOnTargetUpdated);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithNotifyOnTargetUpdated(object notifyOnTargetUpdated)
        {
            return WithSetParameter("NotifyOnTargetUpdated", notifyOnTargetUpdated);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithNotifyOnValidationError(bool notifyOnValidationError)
        {
            return WithSetParameter("NotifyOnValidationError", notifyOnValidationError);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithNotifyOnValidationError(object notifyOnValidationError)
        {
            return WithSetParameter("NotifyOnValidationError", notifyOnValidationError);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithStringFormat(string stringFormat)
        {
            return WithSetParameter("StringFormat", stringFormat);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithStringFormat(object stringFormat)
        {
            return WithSetParameter("StringFormat", stringFormat);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithTargetNullValue(object targetNullValue)
        {
            return WithSetParameter("TargetNullValue", targetNullValue);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithUpdateSourceTrigger(DataUpdateSourceTrigger updateSourceTrigger)
        {
            return WithSetParameter("UpdateSourceTrigger", updateSourceTrigger);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithUpdateSourceTrigger(object updateSourceTrigger)
        {
            return WithSetParameter("UpdateSourceTrigger", updateSourceTrigger);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithValidatesOnDataErrors(bool validatesOnDataErrors)
        {
            return WithSetParameter("ValidatesOnDataErrors", validatesOnDataErrors);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithValidatesOnDataErrors(object validatesOnDataErrors)
        {
            return WithSetParameter("ValidatesOnDataErrors", validatesOnDataErrors);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithValidatesOnExceptions(bool validatesOnExceptions)
        {
            return WithSetParameter("ValidatesOnExceptions", validatesOnExceptions);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithValidatesOnExceptions(object validatesOnExceptions)
        {
            return WithSetParameter("ValidatesOnExceptions", validatesOnExceptions);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithValidationRules(object validationRules)
        {
            return WithSetParameter("ValidationRules", validationRules);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithXPath(string xPath)
        {
            return WithSetParameter("XPath", xPath);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBind WithXPath(object xPath)
        {
            return WithSetParameter("XPath", xPath);
        }

    }

    partial class AmmyBindBuilder
    {
        [isukces.code.AutocodeGenerated]
        public AmmyBindBuilder WithBindFromAncestor<T>(int? level = null)
        {
            return this.WithBindFromAncestor(typeof(T), level);
        }

        [isukces.code.AutocodeGenerated]
        public AmmyBindBuilder WithConverterStatic<T>(string propertyName)
        {
            return this.WithConverterStatic(typeof(T), propertyName);
        }

    }
}
