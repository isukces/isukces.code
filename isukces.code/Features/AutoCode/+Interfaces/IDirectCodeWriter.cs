using System;

namespace isukces.code.AutoCode
{
    public interface IDirectCodeWriter
    {
        #region Instance Methods

        void ChangeIndent(int plus);
        void OpenNamespace(string ns);
        string TypeName(Type type);
        void WriteLn(string txt);

        #endregion
    }
}