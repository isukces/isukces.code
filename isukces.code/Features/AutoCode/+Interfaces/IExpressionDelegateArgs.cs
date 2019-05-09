using System;
using isukces.code.interfaces;

namespace isukces.code.AutoCode
{
    public interface IExpressionDelegateArgs : IArgumentsHolder
    {
        ITypeNameResolver Resolver { get; }
    }
}