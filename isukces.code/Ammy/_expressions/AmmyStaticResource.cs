using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public class AmmyStaticResource : IAmmyCodePieceConvertible
    {
        public AmmyStaticResource(string resourceName)
        {
            ResourceName = resourceName;
        }

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            return new SimpleAmmyCodePiece(Code);
        }

        public override string ToString()
        {
            return Code;
        }

        private string Code => $"resource \"{ResourceName}\"";

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public string ResourceName { get; set; }
    }
}