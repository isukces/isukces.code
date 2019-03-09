using isukces.code.interfaces;

namespace isukces.code
{
    public class CsDirectCode : IDirectCode
    {
        public CsDirectCode(string code)
        {
            Code = code;
        }

        public override string ToString()
        {
            return Code;
        }

        public string Code { get; }
    }
}