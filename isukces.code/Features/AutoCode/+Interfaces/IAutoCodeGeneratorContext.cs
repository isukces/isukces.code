#region using

using System;

#endregion

namespace isukces.code.AutoCode
{
    public interface IAutoCodeGeneratorContext : IConfigResolver
    {
        #region Instance Methods

        void AddNamespace(string namepace);
        CsClass GetOrCreateClass(Type type);

        #endregion
    }
}