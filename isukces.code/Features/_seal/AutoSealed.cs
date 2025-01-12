#nullable enable
using System;
using iSukces.Code.Interfaces;

namespace iSukces.Code;

public sealed class AutoSealedAttribute : Attribute
{
    public AutoSealedAttribute(AutoSealedKind kind = AutoSealedKind.Generate)
    {
        Kind = kind;
    }

    public AutoSealedKind Kind { get; }
}

public enum AutoSealedKind
{
    /// <summary>
    ///     Dodaj autokod z sealed
    /// </summary>
    Generate,


    /// <summary>
    ///     Ignoruj przetwarzanie autokodu, programista lub inny generator zrobi to sam
    /// </summary>
    Ignore
}

public static class AutocodeTools
{
    public static void Seal(CsClass cl)
    {
        if (!cl.TrySeal()) return;
        var valueCode = cl.GetTypeName<AutoSealedKind>().Declaration + "." + AutoSealedKind.Ignore;
        var at = CsAttribute.Make<AutoSealedAttribute>(cl)
            .WithArgumentCode(valueCode);
        cl.Attributes.Add(at);
    }
}
