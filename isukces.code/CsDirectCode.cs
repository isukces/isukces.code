using iSukces.Code.Interfaces;

namespace iSukces.Code
{
    public class CsDirectCode : IDirectCode
    {
        public CsDirectCode(string code) => Code = code;

        public override string ToString() => Code;

        public string Code { get; }
    }
}
