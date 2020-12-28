using System;
using System.Collections.Generic;

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
        
        public NonTerminalInfo Root { get; set; }

        public CommentInfo SingleLineComment { get; set; }
        public CommentInfo DelimitedComment  { get; set; }

        public List<NonTerminalInfo> NonTerminals { get; } = new List<NonTerminalInfo>();

        public Type DefaultBaseClass { get; set; }

        public string TargetNamespace { get; set; }

        public List<TerminalInfo> Terminals { get; } = new List<TerminalInfo>();

        public Dictionary<SpecialTerminalKind, string> SpecialTerminals { get; } =
            new Dictionary<SpecialTerminalKind, string>();
    }
}