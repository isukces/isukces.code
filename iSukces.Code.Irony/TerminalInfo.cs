using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public class TerminalInfo : TokenInfo, ICsExpression
    {
        public TerminalInfo(string code, TerminalName name) : base(name) => Code = code;

        public override string GetCode(ITypeNameResolver resolver) => Name.GetCode(resolver);

        public string Code { get; }
    }
}