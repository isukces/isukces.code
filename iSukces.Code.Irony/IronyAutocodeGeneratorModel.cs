using System;
using System.Collections.Generic;
using Irony.Parsing;

namespace iSukces.Code.Irony
{
    public class IronyAutocodeGeneratorModel
    {
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

        public TerminalInfo WithTerm(string code, TerminalName name)
        {
            var termInfo = new TerminalInfo(code, name);
            Terminals.Add(termInfo);
            return termInfo;
        }

        internal AstTypesInfoDelegate GetAstTypesInfoDelegate(ITerminalNameSource src)
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

            var exp = src.GetTerminalName();

            switch (exp)
            {
                case TerminalName tn:
                {
                    foreach (var i in SpecialTerminals)
                        if (i.Value == tn.Name)
                            return AstTypesInfo.BuildFrom(i.Key);

                    return null;
                }
            }

            /*if (Equals(exp, identifier))
                        return q => q.GetTypeName(typeof(IdentifierTerminal));*/
            throw new NotImplementedException();
        }

        public NonTerminalInfo Root { get; set; }

        public CommentInfo SingleLineComment { get; set; }
        public CommentInfo DelimitedComment  { get; set; }

        public List<NonTerminalInfo> NonTerminals { get; } = new List<NonTerminalInfo>();

        public Type DefaultAstBaseClass { get; set; }

        public GrammarNames Names { get; set; }


        public List<TerminalInfo> Terminals { get; } = new List<TerminalInfo>();

        public Dictionary<SpecialTerminalKind, string> SpecialTerminals { get; } =
            new Dictionary<SpecialTerminalKind, string>();

        public NumberOptions CSharpNumberLiteralOptions { get; set; }

        public event EventHandler<OnGetAstTypesInfoDelegateEventArgs> OnGetAstTypesInfoDelegate;

        public sealed class OnGetAstTypesInfoDelegateEventArgs : EventArgs
        {
            public OnGetAstTypesInfoDelegateEventArgs(ITerminalNameSource src) => Source = src;

            public AstTypesInfoDelegate Result { get; set; }

            public ITerminalNameSource Source  { get; }
            public bool                Handled { get; set; }
        }
    }
}