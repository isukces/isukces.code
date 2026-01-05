using System;

namespace iSukces.Code.AutoCode;

public sealed class MaskIntType
{
    private MaskIntType(int bits, string type)
    {
        Bits = bits;
        Type = type;
    }

    public int    Bits { get; }
    public string Type { get; }


    public static MaskIntType Create(int bits)
    {
        switch (bits)
        {
            case > 32:
                return new MaskIntType(64, "ulong");
            case > 16:
                return new MaskIntType(32, "uint");
            case > 8:
                return new MaskIntType(16, "uint");
            default:
                return new MaskIntType(8, "byte");
        }
    }

    public string GetAnd(string variable, ulong value)
    {
        var txt = value.ToCsString();
        return Bits switch
        {
            64 => $"({variable} & {txt}UL) != 0",
            32 => $"({variable} & {txt}u) != 0",
            _ => $"({variable} & {txt}) != 0",
        };
    }

    public string ConvertToString(ulong value)
    {
        var txt = value.ToCsString();
        return Bits switch
        {
            64 => $"{txt}UL",
            32 => $"{txt}u",
            16 => $"(ushort){txt}",
            8 => $"(byte){txt}",
            _ => throw new ArgumentOutOfRangeException(nameof(Bits), Bits, "Invalid bits value")
        };
    }
}
