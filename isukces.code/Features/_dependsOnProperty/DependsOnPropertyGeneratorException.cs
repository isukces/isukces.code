using System;

namespace iSukces.Code
{
    public sealed class DependsOnPropertyGeneratorException:Exception {
        public DependsOnPropertyGeneratorException(string message, Exception? innerException=null)
            : base(message, innerException)
        {
        }
    }
}

