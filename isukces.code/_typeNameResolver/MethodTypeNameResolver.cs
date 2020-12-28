using System;
using iSukces.Code.Interfaces;
using JetBrains.Annotations;

namespace iSukces.Code
{
    public class MethodTypeNameResolver : ITypeNameResolver
    {
        public MethodTypeNameResolver([NotNull] Func<Type, string> method)
        {
            _method = method ?? throw new ArgumentNullException(nameof(method));
        }

        public string GetTypeName(Type type)
        {
            return _method(type);
        }


        private readonly Func<Type, string> _method;
    }
}