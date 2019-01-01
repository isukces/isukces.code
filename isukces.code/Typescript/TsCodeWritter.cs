using System;

namespace isukces.code.Typescript
{
    public class TsCodeWritter : CodeWritter
    {
        public TsCodeWritter() : base(TsLangInfo.Instance)
        {
        }
        
        public bool HeadersOnly { get; set; }

        public void DoWithHeadersOnly(bool temporaryHeadersOnly, Action a)
        {
            var back = HeadersOnly;
            HeadersOnly = temporaryHeadersOnly;
            a?.Invoke();
            HeadersOnly = back;
        }
    }
}