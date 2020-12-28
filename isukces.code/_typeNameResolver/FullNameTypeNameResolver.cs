using System;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;

namespace iSukces.Code
{
    public sealed class FullNameTypeNameResolver : ITypeNameResolver, INamespaceContainer
    {
        private FullNameTypeNameResolver()
        {
        }

        public string GetTypeName(Type type) => GeneratorsHelper.GetTypeName(this, type);

        public bool IsKnownNamespace(string namespaceName) => false;

        public static FullNameTypeNameResolver Instance
        {
            get { return InstanceHolder.SingleInstance; }
        }

        private sealed class InstanceHolder
        {
            public static readonly FullNameTypeNameResolver SingleInstance = new FullNameTypeNameResolver();
        }
    }
}