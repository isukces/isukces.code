using System;
using System.Collections.Generic;
using Irony.Parsing;
using JetBrains.Annotations;

#nullable disable
namespace iSukces.Code.Irony
{
    public class IronyAutocodeGeneratorModel
    {
        [CanBeNull]
        public SequenceRuleBuilder.TokenInfoResult GetTokenInfoByName(ITokenNameSource src)
        {
            var terminalName = src.GetTokenName();
            if (Punctuations.ContainsTerminalName(terminalName))
                return new SequenceRuleBuilder.TokenInfoResult
                {
                    IsNoAst = true
                };
            if (ReservedWords.ContainsTerminalName(terminalName))
                return new SequenceRuleBuilder.TokenInfoResult
                {
                    IsNoAst = false
                };

            foreach (var i in BracketsPairs)
                if (i.Item1 == terminalName.Name || i.Item2 == terminalName.Name)
                    return new SequenceRuleBuilder.TokenInfoResult
                    {
                        IsNoAst = false
                    };
            switch (terminalName)
            {
                case TokenName tn:
                {
                    foreach (var i in SpecialTerminals)
                        if (i.Value == tn.Name)
                            return new SequenceRuleBuilder.TokenInfoResult
                            {
                                SpecialTerminal = i.Key
                            };

                    return null;
                }
            }
        }

        public IronyAutocodeGeneratorModel WithDelimitedComment(string startSymbol, params string[] endSymbols)
        {
            DelimitedComment = new CommentInfo("DelimitedComment", startSymbol, endSymbols);
            return this;
        }

        public IronyAutocodeGeneratorModel WithSingleLineComment(string startSymbol, params string[] endSymbols)
        {
            SingleLineComment = new CommentInfo("SingleLineComment", startSymbol, endSymbols);
            return this;
        }

        public TerminalInfo WithTerm(string code, TokenName name)
        {
            var termInfo = new TerminalInfo(code, name);
            Terminals.Add(termInfo);
            return termInfo;
        }

        internal AstTypesInfoDelegate GetAstTypesInfoDelegate(ITokenNameSource src)
        {
            {
                var h = OnGetAstTypesInfoDelegate;
                if (h != null)
                {
                    var arg = new OnGetAstTypesInfoDelegateEventArgs(src);
                    h(this, arg);
                    if (arg.Handled)
                        return arg.Result;
                }
            }
            switch (src)
            {
                case NonTerminalInfo nti:
                    return AstTypesInfo.BuildFrom(nti, Names);
            }

            var exp = src.GetTokenName();

            switch (exp)
            {
                case TokenName tn:
                {
                    foreach (var i in SpecialTerminals)
                        if (i.Value == tn.Name)
                            return AstTypesInfo.BuildFrom(i.Key);

                    return null;
                }
            }
        }

        public NonTerminalInfo Root { get; set; }

        public CommentInfo SingleLineComment { get; set; }
        public CommentInfo DelimitedComment  { get; set; }

        public List<NonTerminalInfo> NonTerminals { get; } = new List<NonTerminalInfo>();

        public Type DefaultAstBaseClass { get; set; }

        public List<Tuple<string, string>> BracketsPairs { get; } = new List<Tuple<string, string>>();

        public GrammarNames Names { get; set; }


        public TerminalsList Terminals { get; } = new TerminalsList();

        public Dictionary<SpecialTerminalKind, string> SpecialTerminals { get; } =
            new Dictionary<SpecialTerminalKind, string>();

        public NumberOptions     CSharpNumberLiteralOptions { get; set; }
        public TerminalsList     ReservedWords              { get; } = new TerminalsList();
        public TerminalsList     Punctuations               { get; } = new TerminalsList();
        public IDoEvaluateHelper DoEvaluateHelper           { get; set; }

        public event EventHandler<OnGetAstTypesInfoDelegateEventArgs> OnGetAstTypesInfoDelegate;

        public sealed class OnGetAstTypesInfoDelegateEventArgs : EventArgs
        {
            public OnGetAstTypesInfoDelegateEventArgs(ITokenNameSource src) => Source = src;

            public AstTypesInfoDelegate Result { get; set; }

            public ITokenNameSource Source  { get; }
            public bool             Handled { get; set; }
        }
    }
}

