using System;
using isukces.code.interfaces;

namespace isukces.code
{
    public static class CsMethodExtensions
    {
      
        public static CsMethod WithBody(this CsMethod method, string body)
        {
            method.Body = body;
            return method;
        }

        public static CsMethod WithBody(this CsMethod method, CodeFormatter code)
        {
            return WithBody(method, code?.Text);
        }

        public static CsMethod WithOverride(this CsMethod method, bool isOverride = true)
        {
            method.IsOverride = isOverride;
            return method;
        }
     
    }
}