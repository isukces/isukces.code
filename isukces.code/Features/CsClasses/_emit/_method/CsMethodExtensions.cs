using System;

namespace iSukces.Code;

public static class CsMethodExtensions
{
    public static CsMethod WithAbstract(this CsMethod method, bool isAbstract = true)
    {
        method.Overriding = isAbstract ? OverridingType.Abstract : OverridingType.None;
        return method;
    }

    public static CsMethod WithBody(this CsMethod method, string body)
    {
        method.Body = body;
        return method;
    }

    public static CsMethod WithBody(this CsMethod method, CodeWriter? code) => WithBody(method, code?.Code);

    public static CsMethod WithOverride(this CsMethod method, bool isOverride = true)
    {
        method.Overriding = isOverride ? OverridingType.Override : OverridingType.None;
        return method;
    }

    public static CsMethod WithParameter(this CsMethod method, CsMethodParameter parameter)
    {
        method.Parameters.Add(parameter);
        return method;
    }
        
    public static CsMethod WithParameter(this CsMethod method, string name, CsType type = default, string? description = null)
    {
        var parameter = new CsMethodParameter(name, type, description);
        method.Parameters.Add(parameter);
        return method;
    }
        
    public static CsMethod WithVirtual(this CsMethod method, bool isVirtual = true)
    {
        method.Overriding = isVirtual ? OverridingType.Virtual : OverridingType.None;
        return method;
    }
}
