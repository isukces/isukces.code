using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;
using iSukces.Code.CodeWrite;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public abstract class IronyGrammarConfigurator
    {
        public static void AddImportNamespaces(CsFile file)
        {
            file.AddImportNamespace(typeof(StatementListNode));
            file.AddImportNamespace(typeof(NonTerminal));
            file.AddImportNamespace(typeof(InterpretedLanguageGrammar));
            file.AddImportNamespace(typeof(AstContext));
            file.AddImportNamespace("System.Collections.Generic");
            file.AddImportNamespace(typeof(TermListOptions2));
        }

        public static readonly ICsExpression Eos = new DirectCode("Eos");
        public static readonly ICsExpression NewLine = new WhiteCharCode("NewLine");
        public static readonly ICsExpression NewLinePlus = new WhiteCharCode("NewLinePlus");
        public static readonly ICsExpression NewLineStar = new WhiteCharCode("NewLineStar");
        public static readonly ICsExpression Eof = new DirectCode("Eof");
        
    }

    public abstract class IronyAutocodeConfigurator<TBaseAstType> : IronyGrammarConfigurator
    {
        protected IronyAutocodeConfigurator(GrammarNames names) => Names = names;

        protected IronyAutocodeConfigurator(string namespaceName, string name)
        {
            var grammarClass = new NamespaceAndName(namespaceName, name);
            Names = GrammarNames.Make(grammarClass);
        }

        public IronyAutocodeGenerator Build()
        {
            BuildInternal();
            var gen = Generator;
            gen.Cfg.DefaultAstBaseClass = typeof(TBaseAstType);
            return gen;
        }

        protected NonTerminalInfo AddNonTerminal(string code) => AddNonTerminal(NonTerminalInfo.Parse(code));

        protected NonTerminalInfo AddNonTerminal(NonTerminalInfo info)
        {
            Generator.Cfg.NonTerminals.Add(info);
            AfterAdd(info);
            return info;
        }

        /*protected ICsExpression StringToTerm(string text)
        {
            return new ToTermFunctionCall(text);
        }*/

        protected NonTerminalInfo AddOptionalFrom(TokenInfo info) => AddNonTerminal(info.CreateOptional());


        protected TerminalInfo AddTerminal(string code, string name) =>
            Generator.Cfg.WithTerm(code, new TerminalName(name));

        protected virtual void AfterAdd(NonTerminalInfo info)
        {
        }

        protected abstract void BuildInternal();

        protected GrammarNames Names { get; }


        /*
        public ITypeNameResolver Resolver
        {
            get
            {
                if (!(_resolver is null)) return _resolver;
                _resolver = new CsNamespace(null, Generator.Cfg.Namespaces);
                return _resolver;
            }
        }
        */


        protected IronyAutocodeGenerator Generator
        {
            get
            {
                if (!(_generator is null))
                    return _generator;
                var ns = Names;
                _generator = new IronyAutocodeGenerator(ns);
                return _generator;
            }
        }


        private IronyAutocodeGenerator _generator;
        private ITypeNameResolver _resolver;
    }
}