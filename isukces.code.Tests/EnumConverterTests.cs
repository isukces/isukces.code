using System;
using System.Linq;
using iSukces.Code.Interfaces;
using Xunit;

namespace iSukces.Code.Tests;

public class EnumConverterTests
{
    [Fact]
    public void T01_Should_Convert_Zero()
    {
        var e = CsEnumHelper.Get(typeof(TestEnum));
        Assert.Equal("None", e.Zero);
    }

    [Theory]
    [InlineData(1, "x.One")]
    [InlineData(2, "x.Two")]
    [InlineData(3, "x.Both")]
    [InlineData(4, "x.X")]
    [InlineData(5, "x.SomeCombination")]
    [InlineData(6, "x.Two|x.X")]
    [InlineData(7, "x.All")]
    [InlineData(8, "(x)8")]
    [InlineData(8 + 5, "x.SomeCombination|(x)8")]
    public void T02_Should_Convert(int value, string exp)
    {
        var e   = CsEnumHelper.Get(typeof(TestEnum));
        var q   = e.GetFlagStrings((TestEnum)value, "x").ToArray();
        var got = string.Join("|", q);
        Assert.Equal(exp, got);
    }

    [Flags]
    private enum TestEnum
    {
        None = 0,

        One = 1,
        Two = 2,
        Both = One | Two,
        X = 4,
        SomeCombination = X | One,
        All = 7
    }
}