using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public class MixinBuilder<T> : IAmmyContainer, IAmmyCodePieceConvertible
    {
        public MixinBuilder(string mixinName)
        {
            WrappedMixin = new Mixin(mixinName, typeof(T));
        }

        public MixinBuilder<T> WithProperty(Expression<Func<T, object>> propertyNameE, object value)
        {
            var propertyName = ExpressionTools.GetBindingPath(propertyNameE);
            this.WithProperty(propertyName, value);
            return this;
        }


        public MixinBuilder<T> WithPropertyAncestorBind<T1>(
            Expression<Func<T, object>> propertyNameE,
            Expression<Func<T1, object>> action,
            params KeyValuePair<string, string>[] bindingSettings)
        {
            var path         = ExpressionTools.GetBindingPath(action);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameE);
            WithPropertyAncestorBind(propertyName, path, bindingSettings);
            return this;
        }


        public MixinBuilder<T> WithPropertyAncestorBind(string propertyName, string path,
            params KeyValuePair<string, string>[] bindingSettings)
        {
            return this.WithPropertyAncestorBind(propertyName, path, DefaultAncestorType, bindingSettings);
        }


        public MixinBuilder<T> WithPropertyStaticValue<T1>(Expression<Func<T, T1>> propertyNameE, T1 value)
        {
            var propertyName = ExpressionTools.GetBindingPath(propertyNameE);
            this.WithProperty(propertyName, value);
            return this;
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
}