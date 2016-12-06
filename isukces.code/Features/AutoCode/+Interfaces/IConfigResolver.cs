#region using

using isukces.code.interfaces;

#endregion

namespace isukces.code.AutoCode
{
    public interface IConfigResolver
    {
        #region Instance Methods

        T ResolveConfig<T>() where T : class, IAutoCodeConfiguration, new();

        #endregion
    }
}