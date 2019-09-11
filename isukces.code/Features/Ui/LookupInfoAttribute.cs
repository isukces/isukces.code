using System;

namespace isukces.code.Ui
{
    /// <summary>
    ///     Decorate enum type with LookupInfoAttribute in order to point type that implements IEnumLookupProvider
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum)]
    public class LookupInfoAttribute : Attribute
    {
        public LookupInfoAttribute(Type lookupProvider) => LookupProvider = lookupProvider;

        public Type LookupProvider { get; }
    }
}