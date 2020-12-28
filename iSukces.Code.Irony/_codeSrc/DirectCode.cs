using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public sealed class DirectCode : ICsExpression
    {
        public DirectCode(string code)
        {
            _code = code;
        }

        public string GetCode(ITypeNameResolver resolver)
        {
            return _code;
        }

        private readonly string _code;
    }
}