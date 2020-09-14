using System;

namespace iSukces.Code.Interfaces
{
    public interface ITsCodeWriter : ICodeWriter
    {
        bool HeadersOnly { get; set; }
    }

    public static class TsCodeWritterExt
    {
        public static void DoWithHeadersOnly(this ITsCodeWriter src, bool temporaryHeadersOnly, Action a)
        {
            var back = src.HeadersOnly;
            src.HeadersOnly = temporaryHeadersOnly;
            a?.Invoke();
            src.HeadersOnly = back;
        }
    }
}