using System;

namespace isukces.code
{
    public class RecompileException : Exception
    {
        public RecompileException()
            : base("Source code was changed. Please recompile.")
        {
        }
    }
}