using System.Collections.Generic;
using System.Linq;
using isukces.code.interfaces;

namespace isukces.code
{
    public class CsCodeWriter : CodeWriter, ICsCodeWriter
    {
        public CsCodeWriter() 
            : base(CsLangInfo.Instance)
        {            
        }

    }
}