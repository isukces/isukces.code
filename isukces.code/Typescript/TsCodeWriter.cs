using iSukces.Code.Interfaces;

namespace iSukces.Code.Typescript
{
    public class TsCodeWriter : CodeWriter, ITsCodeWriter
    {
        public TsCodeWriter() : base(TsLangInfo.Instance)
        {
        }
        
        public bool HeadersOnly { get; set; }
      
    }
}