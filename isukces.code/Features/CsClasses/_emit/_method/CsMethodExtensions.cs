namespace iSukces.Code;

public static class CsMethodExtensions
{
    extension(CsMethod method)
    {
        public CsMethod WithAbstract(bool isAbstract = true)
        {
            method.Overriding = isAbstract ? OverridingType.Abstract : OverridingType.None;
            return method;
        }

        public CsMethod WithBody(string body)
        {
            method.Body = body;
            return method;
        }

        public CsMethod WithBody(CodeWriter? code) => WithBody(method, code?.Code);

        public CsMethod WithOverride(bool isOverride = true)
        {
            method.Overriding = isOverride ? OverridingType.Override : OverridingType.None;
            return method;
        }

        public CsMethod WithParameter(CsMethodParameter parameter)
        {
            method.Parameters.Add(parameter);
            return method;
        }

        public CsMethod WithParameter(string name, CsType type = default, string? description = null)
        {
            var parameter = new CsMethodParameter(name, type, description);
            method.Parameters.Add(parameter);
            return method;
        }

        public CsMethod WithVirtual(bool isVirtual = true)
        {
            method.Overriding = isVirtual ? OverridingType.Virtual : OverridingType.None;
            return method;
        }
    }
}
