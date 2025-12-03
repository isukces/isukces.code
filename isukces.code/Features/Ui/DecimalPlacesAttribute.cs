using System;
using System.Diagnostics;

namespace iSukces.Code.Ui;

[AttributeUsage(AttributeTargets.Property)]
[Conditional("AUTOCODE_ANNOTATIONS")]
public class DecimalPlacesAttribute : Attribute
{
    public DecimalPlacesAttribute(int decimalPlaces) => DecimalPlaces = decimalPlaces;

    public int DecimalPlaces { get; }

    public string Format
    {
        get
        {
            const string format = "#,0";
            if (DecimalPlaces > 0)
                return format + "." + new string('0', DecimalPlaces);
            return format;
        }
    }
}