using System.Collections.Generic;
using iSukces.Code.Interfaces.Ammy;

namespace iSukces.Code.Ammy
{
    public class ComplexAmmyCodePiece : IComplexAmmyCodePiece
    {
        public ComplexAmmyCodePiece(IReadOnlyList<IAmmyCodePiece> codePieces, string openingCode, AmmyBracketKind brackets = AmmyBracketKind.Mustache)
        {
            _codePieces = codePieces;
            OpeningCode = openingCode;
            Brackets = brackets;
        }

        public IReadOnlyList<IAmmyCodePiece> GetNestedCodePieces()
        {
            return _codePieces;
        }

        public string GetOpeningCode()
        {
            return OpeningCode;
        }

        public AmmyBracketKind Brackets { get;  }

        public string OpeningCode { get; }

        public bool WriteInSeparateLines { get; set; }
        private readonly IReadOnlyList<IAmmyCodePiece> _codePieces;
    }
}