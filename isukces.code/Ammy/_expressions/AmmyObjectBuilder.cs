using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public class AmmyObjectBuilder<T> : AmmyContainerBase, IAmmyCodePieceConvertible
    {
        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            return ToComplexAmmyCode(ctx);
        }

        public IComplexAmmyCodePiece ToComplexAmmyCode(IConversionCtx ctx)
        {
            return this.ToComplexAmmyCode(ctx, ctx.TypeName<T>());
        }
 
    }
}