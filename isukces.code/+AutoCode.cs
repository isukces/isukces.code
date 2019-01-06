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
        public MixinBuilder<TPropertyBrowser> WithPropertyAncestorBind<TAncestor>(Expression<Func<TPropertyBrowser, object>> propertyNameExpression, Expression<Func<TPropertyBrowser, object>> bindToPathExpression, [CanBeNull] KeyValuePair<string, string>[] bindingSettings = null)
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
        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyAncestorBind<TAncestor>(Expression<Func<TPropertyBrowser, object>> propertyNameExpression, Expression<Func<TPropertyBrowser, object>> bindToPathExpression, [CanBeNull] KeyValuePair<string, string>[] bindingSettings = null)
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

    }
}
