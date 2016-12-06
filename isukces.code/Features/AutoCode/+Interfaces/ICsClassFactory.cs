#region using

using System;

#endregion

namespace isukces.code.AutoCode
{
    public interface IAutoCodeGenerator
    {
        #region Instance Methods

        void Generate(Type type, IAutoCodeGeneratorContext context);

        #endregion
    }
}