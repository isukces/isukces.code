using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public class TerminalInfo : TokenInfo, ICsExpression
    {
        public TerminalInfo(string code, TokenName name) : base(name) => Code = code;

        public override string GetCode(ITypeNameResolver resolver) => Name.GetCode(resolver);
        public override TokenNameTarget GetTokenNameIsNonterminal() => TokenNameTarget.Nonterminal;

        public override string ToString()
        {
            if (Code == Name.Name)
                return "term: " + Code;
            return "term: " + Code + " as " + Name.Name;
        }

        public string Code { get; }
    }
}