using System;

namespace iSukces.Code.Ui;

public struct UiWidth
{
    public UiWidth(int value, UiWidthKind unit)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Value must be >= 0");
        Value = value;
        Unit  = unit;
    }

    public static implicit operator UiWidth(int value)
    {
        return new UiWidth(value, UiWidthKind.Pixels);
    }

    public override string ToString()
    {
        if (Value == 0) return "0";
        return Unit switch
        {
            UiWidthKind.None => "",
            UiWidthKind.Pixels => Value.ToCsString(),
            UiWidthKind.Stars when Value == 1 => "*",
            UiWidthKind.Stars => Value.ToCsString() + "*",
            _ => "?"
        };
    }

    public int         Value         { get; }
    public UiWidthKind Unit          { get; }
    public bool        IsEmpty       => Unit == UiWidthKind.None;
    public bool        IsEmptyOrZero => Unit == UiWidthKind.None || Value == 0;
}

public enum UiWidthKind
{
    None,
    Pixels,
    Stars
}
