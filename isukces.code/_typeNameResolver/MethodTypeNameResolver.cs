using System;
using iSukces.Code.Interfaces;

namespace iSukces.Code
{
    public class MethodTypeNameResolver : ITypeNameResolver
    {
        public MethodTypeNameResolver(Func<Type, CsType> method)
        {
            _method = method ?? throw new ArgumentNullException(nameof(method));
        }

        public CsType GetTypeName(Type type)
        {
            return _method(type);
        }


        private readonly Func<Type, CsType> _method;
    }
}
