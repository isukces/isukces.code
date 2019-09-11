using System;

namespace isukces.code.Ui
{
    public interface IEnumLookupProvider
    {
        string GetDisplayMemberPath();
        string GetSelectedValuePath();
        Tuple<Type, string> GetSourceStaticProperty();
    }
}