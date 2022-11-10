#if AMMY
using iSukces.Code.Interfaces;
using iSukces.Code.Interfaces.Ammy;

namespace iSukces.Code.Ammy
{
    public class ElementNameBindingSource : IAmmyCodePieceConvertible
    {
        public ElementNameBindingSource(string elementName)
        {
            _elementName = elementName;
        }

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            var txt = _elementName.CsEncode();
            return new SimpleAmmyCodePiece(txt);
        }

        private readonly string _elementName;
    }
}
#endif