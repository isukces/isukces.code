using System;

namespace isukces.code.interfaces
{
    public interface ITsCodeWritter : ICodeWritter
    {
        bool HeadersOnly { get; set; }
    }

    public static class TsCodeWritterExt
    {
        public static void DoWithHeadersOnly(this ITsCodeWritter src, bool temporaryHeadersOnly, Action a)
        {
            var back = src.HeadersOnly;
            src.HeadersOnly = temporaryHeadersOnly;
            a?.Invoke();
            src.HeadersOnly = back;
        }
    }
}