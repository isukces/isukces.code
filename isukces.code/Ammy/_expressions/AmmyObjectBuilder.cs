using System;
using System.Linq.Expressions;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public partial class AmmyObjectBuilder<TPropertyBrowser> : AmmyContainerBase, IAmmyCodePieceConvertible        
    {
        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            return ToComplexAmmyCode(ctx);
        }

        public IComplexAmmyCodePiece ToComplexAmmyCode(IConversionCtx ctx)
        {
            return this.ToComplexAmmyCode(ctx, ctx.TypeName<TPropertyBrowser>());
        }
    }


    // === Extension methods are to complicated (3 parameters)
    public partial class AmmyObjectBuilder<TPropertyBrowser>
    {
   
        public AmmyObjectBuilder<TPropertyBrowser> WithProperty<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, object v)
        {
            var mi = AmmyHelper.GetMemberInfo(func);
            this.WithProperty(mi.Member.Name, v);
            return this;
        }

        public AmmyObjectBuilder<TPropertyBrowser> WithPropertyStaticValue<TValue>(Expression<Func<TPropertyBrowser, TValue>> func, TValue v)
        {
            var mi = AmmyHelper.GetMemberInfo(func);
            this.WithProperty(mi.Member.Name, v);
            return this;
        }
    }
}