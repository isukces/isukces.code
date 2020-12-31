using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public class SimpleCodeExpression : ICsExpression
    {
        public SimpleCodeExpression(string code) => Code = code;

        public string GetCode(ITypeNameResolver resolver) => Code;

        public string Code { get; }
    }
}