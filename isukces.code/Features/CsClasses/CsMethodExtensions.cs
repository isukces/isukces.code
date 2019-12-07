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

        public static CsMethod WithBody(this CsMethod method, CodeWriter code)
        {
            return WithBody(method, code?.Code);
        }

        public static CsMethod WithOverride(this CsMethod method, bool isOverride = true)
        {
            method.IsOverride = isOverride;
            return method;
        }
     
        public static CsMethod WithParameter(this CsMethod method, CsMethodParameter parameter)
        {
            method.Parameters.Add(parameter); 
            return method;
        }
    }
}