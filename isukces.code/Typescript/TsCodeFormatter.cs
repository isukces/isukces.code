using System;

namespace isukces.code.Typescript
{
    public class TsCodeFormatter : CodeFormatter
    {
        public TsCodeFormatter() : base(TsLangInfo.Instance)
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