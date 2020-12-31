using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public sealed class DirectCode : ICsExpression
    {
        public DirectCode(string code) => Code = code;

        public string GetCode(ITypeNameResolver resolver) => Code;

        public string Code { get; }
    }
}