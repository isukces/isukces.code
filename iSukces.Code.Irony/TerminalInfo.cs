using iSukces.Code.Interfaces;
using iSukces.Code.Irony._codeSrc;

namespace iSukces.Code.Irony
{
    public class TerminalInfo:TokenInfo, ICsExpression
    {
        public TerminalInfo(string code, TerminalName name):base(name)
        {
            Code = code;
        }

        public string       Code { get; }
       
        public override string GetCode(ITypeNameResolver resolver)
        {
            return Name.GetCode(resolver);
        }
    }
}