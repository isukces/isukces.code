using iSukces.Code.Interfaces;
using iSukces.Code.Irony._codeSrc;

namespace iSukces.Code.Irony
{
    public abstract class TokenInfo : ICsExpression
    {
        protected TokenInfo(TerminalName name)
        {
            Name = name;
        }

        public abstract string GetCode(ITypeNameResolver resolver);

        public TerminalName Name { get; }
        
        public NonTerminalInfo CreateOptional()
        {
            var info1 = new NonTerminalInfo(new TerminalName(Name.Name + "_optional"))
                .AsOptional(this);
            return info1;
        }

    }
}