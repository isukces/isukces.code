using System;
using System.Diagnostics;

namespace iSukces.Code.Ui;

/// <summary>
///     Decorate enum type with LookupInfoAttribute in order to point type that implements IEnumLookupProvider
/// </summary>
[AttributeUsage(AttributeTargets.Enum)]
[Conditional("AUTOCODE_ANNOTATIONS")]
public class LookupInfoAttribute : Attribute
{
    public LookupInfoAttribute(Type lookupProvider) => LookupProvider = lookupProvider;

    public Type LookupProvider { get; }
}