using System;
using iSukces.Code.Interfaces;

#nullable disable
namespace iSukces.Code.Irony
{
    public class MethodTypeNameProvider : ITypeNameProvider
    {
        public MethodTypeNameProvider(Func<ITypeNameResolver, string, FullTypeName> func) => _func = func;

        public FullTypeName GetTypeName(ITypeNameResolver resolver, string baseNamespace) =>
            _func(resolver, baseNamespace);


        private readonly Func<ITypeNameResolver, string, FullTypeName> _func;
    }
}

