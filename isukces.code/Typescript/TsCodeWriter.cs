using System;
using isukces.code.interfaces;

namespace isukces.code.Typescript
{
    public class TsCodeWriter : CodeWriter, ITsCodeWriter
    {
        public TsCodeWriter() : base(TsLangInfo.Instance)
        {
        }
        
        public bool HeadersOnly { get; set; }
      
    }
}