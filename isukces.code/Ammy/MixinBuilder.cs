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
            WrappedMixin = new AmmyMixin(mixinName, typeof(TPropertyBrowser));
        }

        public MixinBuilder(AmmyMixin mixin)
        {
            if (mixin.MixinTargetType != typeof(TPropertyBrowser))
                throw new Exception("Invalid mixin type");
            WrappedMixin = mixin;
        }

        public MixinBuilder<TPropertyBrowser> WithProperty(Expression<Func<TPropertyBrowser, object>> propertyNameE,
            object value)
        {
            var propertyName = ExpressionTools.GetBindingPath(propertyNameE);
            this.WithProperty(propertyName, value);
            return this;
        }

        IAmmyCodePiece IAmmyCodePieceConvertible.ToAmmyCode(IConversionCtx ctx) => WrappedMixin.ToAmmyCode(ctx);
        
        public AmmyMixin WrappedMixin { get; }
        
        IDictionary<string, object> IAmmyPropertyContainer.Properties
        {
            get { return WrappedMixin.Properties; }
        }

        private IDictionary<string, object> CustomData { get; } = new Dictionary<string, object>();

        IList<object> IAmmyContentItemsContainer.ContentItems
        {
            get { return WrappedMixin.ContentItems; }
        }
    }
}