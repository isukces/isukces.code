using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public class SimpleCodeExpression : ICsExpression
    {
        public string Code { get; }

        public SimpleCodeExpression(string code)
        {
            Code = code;
        }

        public string GetCode(ITypeNameResolver resolver)
        {
            return Code;
        }
    }
}