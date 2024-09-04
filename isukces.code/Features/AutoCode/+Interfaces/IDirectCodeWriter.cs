#nullable enable
using System;

namespace iSukces.Code.AutoCode
{
    public interface IDirectCodeWriter
    {
        void ChangeIndent(int plus);
        void OpenNamespace(string ns);
        string TypeName(Type type);
        void WriteLn(string txt);
    }
}