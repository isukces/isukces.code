using isukces.code.interfaces;

namespace isukces.code.AutoCode
{
    public interface IConfigResolver
    {
        T ResolveConfig<T>() where T : class, IAutoCodeConfiguration, new();
    }
}