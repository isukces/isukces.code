using System.Collections.Generic;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Wpf.Ammy
{
    public class ComplexAmmyCodePiece : IComplexAmmyCodePiece
    {
        public ComplexAmmyCodePiece(IReadOnlyList<IAmmyCodePiece> codePieces, string openingCode)
        {
            _codePieces = codePieces;
            OpeningCode = openingCode;
        }

        public IReadOnlyList<IAmmyCodePiece> GetNestedCodePieces()
        {
            return _codePieces;
        }

        public string GetOpeningCode()
        {
            return OpeningCode;
        }

        public string OpeningCode { get; set; }

        public bool WriteInSeparateLines { get; set; }
        private readonly IReadOnlyList<IAmmyCodePiece> _codePieces;
    }
}