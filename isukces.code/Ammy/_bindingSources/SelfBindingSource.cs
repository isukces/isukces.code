using iSukces.Code.Interfaces.Ammy;

namespace iSukces.Code.Ammy
{
    public sealed class SelfBindingSource : IAmmyCodePieceConvertible
    {
        private SelfBindingSource()
        {
        }

        public override string ToString() => "$this";

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx) => new SimpleAmmyCodePiece("$this");

        public static SelfBindingSource Instance
        {
            get { return InstanceHolder.SingleInstance; }
        }

        private sealed class InstanceHolder
        {
            public static readonly SelfBindingSource SingleInstance = new SelfBindingSource();
        }
    }
}