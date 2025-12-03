using System;
using System.Diagnostics;

namespace iSukces.Code.Interfaces;

// [Conditional("AUTOCODE_ANNOTATIONS")]
public class _PropertyInfoAttribute : Attribute
{
    /// <summary>
    /// typ własności używany wewnętrznie (zwłaszcza jeśli udostępnianym typem jest interfejs)
    /// </summary>
    public Type PreferredRealType { get; set; }

    /// <summary>
    /// pilnuj, aby własność nie była NULL - domyślnie TRUE
    /// </summary>
    public bool NotNull { get; set; } = true;

    /// <summary>
    /// jeśli własność jest string to użyj trim, wymusza działanie notNull
    /// </summary>
    public bool Trim { get; set; }

    private string description = string.Empty;
    /// <summary>
    /// opis własności
    /// </summary>
    public string Description
    {
        get { return description; }
        set { if (value == (object?)null) value = string.Empty; description = value; }
    }

    /// <summary>
    /// własność ustawiana tylko w konstruktorze
    /// </summary>
    public bool Persist { get; set; }
}