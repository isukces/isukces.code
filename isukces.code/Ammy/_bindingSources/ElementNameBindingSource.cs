using isukces.code.interfaces;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
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