using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public abstract class TokenInfo : ICsExpression, ITokenNameSource
    {
        protected TokenInfo(TokenName name) => Name = name;

        public NonTerminalInfo CreateOptional()
        {
            var info1 = new NonTerminalInfo(new TokenName(Name.Name + "_optional"))
                .AsOptional(this);
            return info1;
        }

        public abstract string GetCode(ITypeNameResolver resolver);

        public TokenName GetTokenName() => Name;
        public abstract TokenNameTarget GetTokenNameIsNonterminal();

        public TokenName Name { get; }

        public TokenCreationInfo CreationInfo { get; } = new TokenCreationInfo();
    }

    public class TokenCreationInfo
    {
        public ConstructorBuilder DataConstructor { get; set; }
        public CsClass            AstClass        { get; set; }
        public CsClass            DataClass       { get; set; }
    }
}