using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public partial class AmmyObjectBuilder<TPropertyBrowser> : AmmyContainerBase, 
        IAmmyObjectBuilder<TPropertyBrowser>
    {
        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            return ToComplexAmmyCode(ctx);
        }

        public IComplexAmmyCodePiece ToComplexAmmyCode(IConversionCtx ctx)
        {
            return this.ToComplexAmmyCode(ctx, ctx.TypeName<TPropertyBrowser>());
        }
        
        /// <summary>
        /// Additional information used by custom generators
        /// </summary>
        public Dictionary<string, object> UserFlags { get; } = new Dictionary<string, object>(); 
    }

}