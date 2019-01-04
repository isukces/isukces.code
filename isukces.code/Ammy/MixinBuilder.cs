using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public partial class MixinBuilder<TPropertyBrowser> : IAmmyContainer, IAmmyCodePieceConvertible
    {
        public MixinBuilder(string mixinName)
        {
            WrappedMixin = new Mixin(mixinName, typeof(TPropertyBrowser));
        }

        public MixinBuilder<TPropertyBrowser> WithProperty(Expression<Func<TPropertyBrowser, object>> propertyNameE,
            object value)
        {
            var propertyName = ExpressionTools.GetBindingPath(propertyNameE);
            this.WithProperty(propertyName, value);
            return this;
        }


        public MixinBuilder<TPropertyBrowser> WithPropertyAncestorBind<T1>(
            Expression<Func<TPropertyBrowser, object>> propertyNameE,
            Expression<Func<T1, object>> action,
            params KeyValuePair<string, string>[] bindingSettings)
        {
            var path         = ExpressionTools.GetBindingPath(action);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameE);
            WithPropertyAncestorBind(propertyName, path, bindingSettings);
            return this;
        }


        public MixinBuilder<TPropertyBrowser> WithPropertyAncestorBind(string propertyName, string path,
            params KeyValuePair<string, string>[] bindingSettings)
        {
            return this.WithPropertyAncestorBind(propertyName, path, DefaultAncestorType, bindingSettings);
        }


        IAmmyCodePiece IAmmyCodePieceConvertible.ToAmmyCode(IConversionCtx ctx)
        {
            return WrappedMixin.ToAmmyCode(ctx);
        }

        public Type DefaultAncestorType { get; set; }

        public Mixin WrappedMixin { get; }


        IDictionary<string, object> IAmmyPropertyContainer.Properties => WrappedMixin.Properties;

        IList<object> IAmmyContentItemsContainer.ContentItems => WrappedMixin.ContentItems;
    }


    public partial class MixinBuilder<TPropertyBrowser> 
    {
        public MixinBuilder<TPropertyBrowser> WithProperty<TValue>(
            Expression<Func<TPropertyBrowser, TValue>> func, object v)
        {
            var mi = AmmyHelper.GetMemberInfo(func);
            this.WithProperty(mi.Member.Name, v);
            return this;
        }

        public MixinBuilder<TPropertyBrowser> WithPropertyStaticValue<TValue>(
            Expression<Func<TPropertyBrowser, TValue>> func, TValue v)
        {
            var mi = AmmyHelper.GetMemberInfo(func);
            this.WithProperty(mi.Member.Name, v);
            return this;
        }
    }
}