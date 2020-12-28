using iSukces.Code.Interfaces;
using iSukces.Code.Irony._codeSrc;

namespace iSukces.Code.Irony
{
    public abstract class IronyAutocodeConfigurator<TBaseType>
    {

        protected readonly ICsExpression Eos = new DirectCode("Eos");
        protected readonly ICsExpression NewLine = new DirectCode("NewLine");
        protected readonly ICsExpression NewLinePlus = new DirectCode("NewLinePlus");
        protected readonly ICsExpression NewLineStar = new DirectCode("NewLineStar");
        protected readonly ICsExpression Eof= new DirectCode("Eof");
        
        public IronyAutocodeGenerator Build()
        {
            BuildInternal();
            var gen = Generator;
            gen.Cfg.DefaultBaseClass = typeof(TBaseType);
            return gen;
        }

        protected NonTerminalInfo AddNonTerminal(string code)
        {
            return AddNonTerminal(NonTerminalInfo.Parse(code));
        }
        
        /*protected ICsExpression StringToTerm(string text)
        {
            return new ToTermFunctionCall(text);
        }*/

        protected NonTerminalInfo AddOptionalFrom(TokenInfo info)
        {
            return AddNonTerminal(info.CreateOptional());
        }

        protected NonTerminalInfo AddNonTerminal(NonTerminalInfo info)
        {
            Generator.Cfg.NonTerminals.Add(info);
            return info;
        }

        protected abstract void BuildInternal();
 
        
        protected TerminalInfo AddTerminal(string code, string name)
        {
            return Generator.Cfg.WithTerm(code, new TerminalName(name));
        }

        public ITypeNameResolver Resolver
        {
            get
            {
                if (!(_resolver is null)) return _resolver;
                _resolver = new CsNamespace(null, Generator.Cfg.TargetNamespace);
                return _resolver;
            }
        }


        protected IronyAutocodeGenerator Generator
        {
            get
            {
                if (!(_generator is null))
                    return _generator;
                var ns = TargetNamespace;
                _generator = new IronyAutocodeGenerator(ns);
                return _generator;
            }
        }

        protected static string TargetNamespace
        {
            get { return typeof(TBaseType).Namespace; }
        }

        private IronyAutocodeGenerator _generator;
        private ITypeNameResolver _resolver;
    }
}

