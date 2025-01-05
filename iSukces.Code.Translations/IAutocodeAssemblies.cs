using System;

namespace iSukces.Code.Translations
{
    public interface IAutocodeAssemblies
    {
        bool ShouldProcessType(Type type);
    }
}
