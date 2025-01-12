using iSukces.Code.Interfaces;

#nullable disable
namespace iSukces.Code.Irony
{
    public class SimpleCodeExpression : ICsExpression
    {
        public SimpleCodeExpression(string code) => Code = code;

        public string GetCode(ITypeNameResolver resolver) => Code;

        public string Code { get; }
    }
}

