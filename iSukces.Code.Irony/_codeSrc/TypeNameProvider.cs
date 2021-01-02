using System;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public class TypeNameProvider : ITypeNameProvider
    {
        public TypeNameProvider(Type type) => Type = type;

        public FullTypeName GetTypeName(ITypeNameResolver resolver, string baseNamespace) =>
            new FullTypeName(resolver.GetTypeName(Type));

        public override string ToString() => "TypeNameProvider for " + Type;

        public Type Type { get; }
    }
}