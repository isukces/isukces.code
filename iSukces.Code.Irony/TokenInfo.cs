using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public abstract class TokenInfo : ICsExpression, ITerminalNameSource
    {
        protected TokenInfo(TerminalName name) => Name = name;

        public NonTerminalInfo CreateOptional()
        {
            var info1 = new NonTerminalInfo(new TerminalName(Name.Name + "_optional"))
                .AsOptional(this);
            return info1;
        }

        public abstract string GetCode(ITypeNameResolver resolver);

        public TerminalName GetTerminalName() => Name;

        public TerminalName Name { get; }
    }
}