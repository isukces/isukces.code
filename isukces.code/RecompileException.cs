using System;

namespace iSukces.Code
{
    public class RecompileException : Exception
    {
        public RecompileException()
            : base("Source code was changed. Please recompile.")
        {
        }
    }
}
