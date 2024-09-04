#nullable enable
using System;
using Xunit;

namespace iSukces.Code.Tests;

public class PrincipalTests
{
    [Fact]
    public void T01_Should_check_equals_for_nullable_int()
    {
        int? a = null;
        int? b = null;
        int? c = 7;
        Assert.True(Equals(a, b));
        Assert.True(Equals(c, 7));
        Assert.False(Equals(a, 1));
        Assert.False(Equals(1, a));
    }

    [Fact]
    public void T02_Should_check_equals_for_string()
    {
        string? a = null;
        string? b = null;
        string  c = "";
        Assert.True(StringComparer.Ordinal.Equals(a, b));
        Assert.True(StringComparer.Ordinal.Equals(c, ""));
        Assert.True(StringComparer.Ordinal.Equals("a", "a"));
        Assert.False(StringComparer.Ordinal.Equals(a, ""));
        Assert.False(StringComparer.Ordinal.Equals("", a));
    }

    [Theory]
    [InlineData(null, null, 0)]
    [InlineData("", null, 0)]
    [InlineData(null, "", 0)]
    [InlineData("", "", 0)]
    [InlineData("a", null, 97)]
    [InlineData(null, "a", -97)]
    public void T03_Should_check_equals_for_string_with_null_is_empty_option(string a, string b, int expect)
    {
        var result = StringComparer.Ordinal.Compare(a ?? string.Empty, b ?? string.Empty);
        Assert.Equal(expect, result);
    }

}