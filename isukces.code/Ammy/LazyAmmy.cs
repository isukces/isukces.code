using System;
using iSukces.Code.Interfaces.Ammy;

namespace iSukces.Code.Ammy
{
    /// <summary>
    /// Provides value that can be evaluated only when IConversionCtx is available
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class LazyAmmy<T> : IAmmyCodePieceConvertible
    {
        public LazyAmmy(Func<IConversionCtx, T> builder)
        {
            _builder = builder;
        }

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            var obj = _builder(ctx);
            return ctx.AnyToCodePiece(obj);
        }

        private readonly Func<IConversionCtx, T> _builder;
    }
}