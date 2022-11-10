#if AMMY
using iSukces.Code.Interfaces.Ammy;

namespace iSukces.Code.Ammy
{
    public class AmmyStaticResource : IAmmyCodePieceConvertible, IAmmySpecialBindCode
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

        public IComplexAmmyCodePiece GetObjectSyntaxCode(IConversionCtx ctx)
        {
            return new ComplexAmmyCodePiece(new[]
            {
                new SimpleAmmyCodePiece(ResourceName.CsEncode()).WithPropertyNameBefore("ResourceKey"),
            }, "StaticResource");
        }

        private string Code => $"resource \"{ResourceName}\"";

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public string ResourceName { get; set; }
    }
}
#endif