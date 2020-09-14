using iSukces.Code.Interfaces.Ammy;

namespace iSukces.Code.Ammy
{
    public class SimpleAmmyCodePiece : ISimpleAmmyCodePiece
    {
        public SimpleAmmyCodePiece(string code, bool writeInSeparateLines = false)
        {
            Code                 = code;
            WriteInSeparateLines = writeInSeparateLines;
        }

        public override string ToString()
        {
            return "Simple ammy code: " + Code;
        }

        public string Code                 { get; }
        public bool   WriteInSeparateLines { get; set; }
    }
}