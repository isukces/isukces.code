using System;

namespace iSukces.Code
{
    public interface IConverterEventArgs<TSource, TResult>
    {
        #region Properties

        TSource SourceValue { get; }

        bool    Handled { get; set; }
        TResult Result  { get; set; }

        #endregion
    }

    public static class ConverterEventArgsExt
    {
        public static void Handle<TSource, TResult>(this IConverterEventArgs<TSource, TResult> src, TResult textInfo)
        {
            if (src.Handled)
                throw new Exception("already handled");
            src.Handled = true;
            src.Result  = textInfo;
        }
    }
}

