#if AMMY
namespace iSukces.Code.Interfaces.Ammy
{
    public sealed class AmmyNone : IAmmyCodePieceConvertible
    {
        private AmmyNone()
        {
        }

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            return new AmmyNoneCodePiece();
        }

        public static IAmmyCodePieceConvertible Instance => AmmyNoneInstanceHolder.SingleInstance;

        private class AmmyNoneInstanceHolder
        {
            public static readonly AmmyNone SingleInstance = new AmmyNone();
        }
    }

    public sealed class AmmyNoneCodePiece : ISimpleAmmyCodePiece
    {
        public bool   WriteInSeparateLines { get; set; }
        public string Code                 => "none";
    }
}
#endif