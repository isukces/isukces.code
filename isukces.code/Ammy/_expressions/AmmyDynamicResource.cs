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
}