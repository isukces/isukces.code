using System;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public class AmmyDynamicResource : IAmmyCodePieceConvertible
    {
        public AmmyDynamicResource(string resourceName)
        {
            ResourceName = resourceName;
        }

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            return new SimpleAmmyCodePiece("resource dyn \"" + ResourceName + "\"");
        }

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public string ResourceName { get; set; }
    }
    
     
    /*public sealed class AmmyDynamicResourceColor : IAmmyCodePieceConvertible
    {
        private readonly StaticBindingSource _bs;

        public AmmyDynamicResourceColor( StaticBindingSource bs)
        {
            _bs = bs;
        }

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            var piece = _bs.ToAmmyCode(ctx);
            if (piece is SimpleAmmyCodePiece s)
                return new SimpleAmmyCodePiece($"resource dyn {s.Code}");
            throw new NotSupportedException();
        }
    }*/
     
}