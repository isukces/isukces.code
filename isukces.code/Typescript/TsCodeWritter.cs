using System;
using isukces.code.interfaces;

namespace isukces.code.Typescript
{
    public class TsCodeWritter : CodeWritter, ITsCodeWritter
    {
        public TsCodeWritter() : base(TsLangInfo.Instance)
        {
        }
        
        public bool HeadersOnly { get; set; }
      
    }
}