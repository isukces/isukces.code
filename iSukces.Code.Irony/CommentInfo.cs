using System.Collections.Generic;
using System.Linq;
using Irony.Parsing;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public class CommentInfo
    {
        public CommentInfo(string name, string startSymbol, params string[] endSymbols)
        {
            Name        = name;
            StartSymbol = startSymbol;
            EndSymbols  = endSymbols;
        }

        public void AddTo(CsClass csc)
        {
            var tn = csc.GetTypeName<CommentTerminal>();
            var s  = new List<string> {Name, StartSymbol};
            s.AddRange(EndSymbols);
            var args       = string.Join(", ", s.Select(IronyAutocodeGenerator.CsEncode2));
            var constValue = $"new {tn}({args})";
            csc.AddField(Name, tn).WithConstValue(constValue);
        }

        public string   Name        { get; }
        public string   StartSymbol { get; }
        public string[] EndSymbols  { get; }
    }
}