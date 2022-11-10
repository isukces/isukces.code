using iSukces.Code.Interfaces;

namespace iSukces.Code.AutoCode
{
    public interface IExpressionDelegateArgs : IArgumentsHolder
    {
        ITypeNameResolver Resolver { get; }
    }
}