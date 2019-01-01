using System.Collections.Generic;
using System.Linq;
using isukces.code.interfaces;

namespace isukces.code
{
    public class CsCodeWritter : CodeWritter, ICsCodeWritter
    {
        public CsCodeWritter() 
            : base(CsLangInfo.Instance)
        {            
        }

    }
}