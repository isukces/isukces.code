using System;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public class TypeTypeNameProvider : ITypeNameProvider
    {
        public TypeTypeNameProvider(Type type) => Type = type;

        public FullTypeName GetTypeName(ITypeNameResolver resolver, string baseNamespace) =>
            new FullTypeName(resolver.GetTypeName(Type));

        public Type Type { get; }
    }
}