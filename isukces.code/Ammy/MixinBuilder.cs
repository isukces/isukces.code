using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using isukces.code.interfaces.Ammy;
using JetBrains.Annotations;

namespace isukces.code.Ammy
{
    public partial class MixinBuilder<TPropertyBrowser> : IAmmyContainer, IAmmyCodePieceConvertible
    {
        public MixinBuilder(string mixinName)
        {
            WrappedMixin = new AmmyMixin(mixinName, typeof(TPropertyBrowser));
        }

        public MixinBuilder<TPropertyBrowser> WithProperty(Expression<Func<TPropertyBrowser, object>> propertyNameE,
            object value)
        {
            var propertyName = ExpressionTools.GetBindingPath(propertyNameE);
            this.WithProperty(propertyName, value);
            return this;
        }

        
        public MixinBuilder<TPropertyBrowser> WithPropertyAncestorBind<TBindTo>(
            Expression<Func<TPropertyBrowser, object>> propertyNameExpression, 
            Expression<Func<TBindTo, object>> bindToPathExpression, 
            [CanBeNull]KeyValuePair<string, string>[] bindingSettings = null)
        {
            var bindToPath   = ExpressionTools.GetBindingPath(bindToPathExpression);
            var propertyName = ExpressionTools.GetBindingPath(propertyNameExpression);
            return this.WithPropertyAncestorBind(propertyName, bindToPath, typeof(TBindTo), bindingSettings);
        }
       
        IAmmyCodePiece IAmmyCodePieceConvertible.ToAmmyCode(IConversionCtx ctx)
        {
            return WrappedMixin.ToAmmyCode(ctx);
        }
 

        public AmmyMixin WrappedMixin { get; }


        IDictionary<string, object> IAmmyPropertyContainer.Properties => WrappedMixin.Properties;

        IList<object> IAmmyContentItemsContainer.ContentItems => WrappedMixin.ContentItems;
    } 
}