using System.Collections.Generic;

namespace iSukces.Code.Irony
{
    public sealed class TerminalsList : List<TerminalInfo>
    {
        public bool ContainsTerminalName(TokenName tokenName)
        {
            foreach (var i in this)
                if (i.Name == tokenName)
                    return true;

            return false;
        }
    }
}