using iSukces.Code.Interfaces;

#nullable disable
namespace iSukces.Code.Irony
{
    public sealed class DirectCode : ICsExpression
    {
        public DirectCode(string code) => Code = code;

        public string GetCode(ITypeNameResolver resolver) => Code;

        public string Code { get; }
    }
}

