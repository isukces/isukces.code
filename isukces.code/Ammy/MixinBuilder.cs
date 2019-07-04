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
       
        IAmmyCodePiece IAmmyCodePieceConvertible.ToAmmyCode(IConversionCtx ctx)
        {
            return WrappedMixin.ToAmmyCode(ctx);
        }
 

        public AmmyMixin WrappedMixin { get; }


        IDictionary<string, object> IAmmyPropertyContainer.Properties => WrappedMixin.Properties;

        private IDictionary<string, object> CustomData { get; } = new Dictionary<string, object>();

        IList<object> IAmmyContentItemsContainer.ContentItems => WrappedMixin.ContentItems;
    } 
}