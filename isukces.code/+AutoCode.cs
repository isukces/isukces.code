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
        public AmmyBind WithConverterStatic<T>(string propertyName)
        {
            return this.WithConverterStatic(typeof(T), propertyName);
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
