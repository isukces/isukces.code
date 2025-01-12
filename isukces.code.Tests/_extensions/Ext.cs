#nullable disable
using iSukces.Code.Interfaces;

namespace iSukces.Code.Tests;

public static class Ext
{
    public static void MakeCode(this CsClass cl, ICsCodeWriter writer)
    {
        cl.MakeCode(writer, default);
    }
}

