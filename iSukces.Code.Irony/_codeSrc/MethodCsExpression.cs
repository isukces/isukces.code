using System;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public class MethodCsExpression : ICsExpression
    {
        public MethodCsExpression(Func<ITypeNameResolver, string> func)
        {
            _func = func;
        }

        public string GetCode(ITypeNameResolver resolver)
        {
            return _func?.Invoke(resolver);
        }

        private readonly Func<ITypeNameResolver, string> _func;
    }
}