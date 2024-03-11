#nullable enable
using Xunit;

namespace iSukces.Code.Tests;

public sealed class CsTypeTests
{
    [Fact]
    public void T01_Should_convert_int()
    {
        var t = new CsType("int");
        Assert.Equal("int", t.AsString(false));
        Assert.Equal("int", t.AsString(true));

    }

    [Fact]
    public void T02_Should_convert_string()
    {
        var t = new CsType("string")
        {
            Nullable = NullableKind.ReferenceNullable
        };
        Assert.Equal("string", t.AsString(false));
        Assert.Equal("string?", t.AsString(true));
    }

    [Fact]
    public void T03_Should_convert_list_of_nullable_strings()
    {
        var t = new CsType("List")
        {
            // Nullable = NullableKind.ReferenceNullable,
            GenericParamaters = new[]
            {
                new CsType("string")
                {
                    Nullable = NullableKind.ReferenceNullable
                }
            }
        };
        Assert.Equal("List<string>", t.AsString(false));
        Assert.Equal("List<string?>", t.AsString(true));
    }

    [Fact]
    public void T04_Should_convert_nullable_list_of_nullable_strings()
    {
        var t = new CsType("List")
        {
            Nullable = NullableKind.ReferenceNullable,
            GenericParamaters = new[]
            {
                new CsType("string")
                {
                    Nullable = NullableKind.ReferenceNullable
                }
            }
        };
        Assert.Equal("List<string>", t.AsString(false));
        Assert.Equal("List<string?>?", t.AsString(true));
    }


    [Fact]
    public void T05_Should_convert_nullable_array_of_nullable_strings()
    {
        var t = new CsType("string")
            .WithReferenceNullable()
            .MakeArray(1, true);
        Assert.Equal("string[]", t.AsString(false));
        Assert.Equal("string?[]?", t.AsString(true));
    }


    [Fact]
    public void T06_Should_convert_nullable_array_of_strings()
    {
        var t = new CsType("string")
            //.WithReferenceNullable()
            .MakeArray(1, true);
        Assert.Equal("string[]", t.AsString(false));
        Assert.Equal("string[]?", t.AsString(true));
    }

    [Fact]
    public void T07_Should_convert_array_of_nullable_strings()
    {
        var t = new CsType("string")
            .WithReferenceNullable()
            .MakeArray();
        Assert.Equal("string[]", t.AsString(false));
        Assert.Equal("string?[]", t.AsString(true));
    }


    [Fact]
    public void T08_Should_create_reference_nullable()
    {
        var a = new CsType("string");
        var b = a.WithReferenceNullable();
        
        Assert.Equal("string", a.Modern);
        Assert.Equal("string?", b.Modern);
    }
}
