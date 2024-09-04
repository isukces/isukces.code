#nullable enable
using System;

namespace iSukces.Code.Ui
{
    public interface IEnumLookupProvider
    {
        string GetDisplayMemberPath();
        string GetSelectedValuePath();
        Tuple<Type, string> GetSourceStaticProperty();
    }
}