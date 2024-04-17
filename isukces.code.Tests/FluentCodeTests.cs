using System;
using Xunit;
using Xunit.Abstractions;

namespace iSukces.Code.Tests;

public class FluentCodeTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public FluentCodeTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void T01_Should_create_fluent_code()
    {
        var uu = new FluentCode
        {
            FirstLineStart = "EditedRow = new X {",
            Ending = "};",
            EmptyCode = "EditedRow = new X { };",
            NextLineStart = ","
        };
        var cs = new CsCodeWriter();
        uu.Write(cs);
        var actual = cs.Code.Trim();
        _testOutputHelper.WriteLine(actual);
        const string expected = @"
EditedRow = new X { };
";
        Assert.Equal(expected.Trim(), actual);
    }

    [Fact]
    public void T02_Should_create_fluent_code()
    {
        var uu = new FluentCode
        {
            FirstLineStart = "EditedRow = new X {",
            Ending = "};",
            EmptyCode = "??",
            NextLineStart = ","
        };
        uu.AddLine("Id = 1");
        uu.AddLine("X = 2");
        var cs = new CsCodeWriter();
        uu.Write(cs);
        var actual = cs.Code.Trim();
        _testOutputHelper.WriteLine(actual);
        const string expected = @"
EditedRow = new X {Id = 1
    ,X = 2};
";
        Assert.Equal(expected.Trim(), actual);
    }

    [Fact]
    public void T03_Should_create_fluent_code()
    {
        var uu = new FluentCode
        {
            FirstLineStart = "EditedRow = new X {",
            Ending = "};",
            EmptyCode = "??",
            NextLineStart = "",
            Separator = ",",
            EndingStyle = FluentCodeEndingStyle.EndingAfterDecIndent
        };
        uu.AddLine("Id = 1");
        var cs = new CsCodeWriter();
        uu.Write(cs);
        var actual = cs.Code.Trim();
        _testOutputHelper.WriteLine(actual);
        const string expected = @"
EditedRow = new X {
    Id = 1
};
";
        Assert.Equal(expected.Trim(), actual);
    }

    [Fact]
    public void T04_Should_create_fluent_code()
    {
        var uu = new FluentCode
        {
            FirstLineStart = "EditedRow = new X {",
            Ending = "};",
            EmptyCode = "??",
            NextLineStart = "",
            Separator = ",",
            EndingStyle = FluentCodeEndingStyle.EndingAfterDecIndent
        };
        uu.AddLine("Id = 1");
        uu.AddLine("X = 2");
        var cs = new CsCodeWriter();
        uu.Write(cs);
        var actual = cs.Code.Trim();
        _testOutputHelper.WriteLine(actual);
        const string expected = @"
EditedRow = new X {
    Id = 1,
    X = 2
};
";
        Assert.Equal(expected.Trim(), actual);
    }

    [Fact]
    public void T05a_Should_create_SetCreateObject()
    {
        var uu = new FluentCode()
            .SetCreateObject("EditedRow", "new X", true, true);
        uu.AddLine("Id = 1");
        uu.AddLine("X = 2");
        var cs = new CsCodeWriter();
        uu.Write(cs);
        var actual = cs.Code.Trim();
        _testOutputHelper.WriteLine(actual);
        const string expected = @"
EditedRow = new X {
    Id = 1,
    X = 2
};
";
        Assert.Equal(expected.Trim(), actual);
    }

    [Fact]
    public void T05b_Should_create_SetCreateObject()
    {
        var uu = new FluentCode()
            .SetCreateObject("EditedRow", "new X", true, true);

        var cs = new CsCodeWriter();
        uu.Write(cs);
        var actual = cs.Code.Trim();
        _testOutputHelper.WriteLine(actual);
        const string expected = "EditedRow = new X();";
        Assert.Equal(expected.Trim(), actual);
    }


    [Fact]
    public void T05c_Should_create_SetCreateObject()
    {
        var uu = new FluentCode()
            .SetCreateObject("EditedRow", "new X", false, true);
        uu.AddLine("Id = 1");
        uu.AddLine("X = 2");
        var cs = new CsCodeWriter();
        uu.Write(cs);
        var actual = cs.Code.Trim();
        _testOutputHelper.WriteLine(actual);
        const string expected = @"
EditedRow = new X {
    Id = 1,
    X = 2
}
";
        Assert.Equal(expected.Trim(), actual);
    }

    [Fact]
    public void T05d_Should_create_SetCreateObject()
    {
        var uu = new FluentCode()
            .SetCreateObject("EditedRow", "new X", false, true);

        var cs = new CsCodeWriter();
        uu.Write(cs);
        var actual = cs.Code.Trim();
        _testOutputHelper.WriteLine(actual);
        const string expected = "EditedRow = new X()";
        Assert.Equal(expected.Trim(), actual);
    }


    [Fact]
    public void T05e_Should_create_SetCreateObject()
    {
        var uu = new FluentCode()
            .SetCreateObject("EditedRow", "new X(1, 2)", false, false);
        uu.AddLine("Id = 1");
        uu.AddLine("X = 2");
        var cs = new CsCodeWriter();
        uu.Write(cs);
        var actual = cs.Code.Trim();
        _testOutputHelper.WriteLine(actual);
        const string expected = @"
EditedRow = new X(1, 2) {
    Id = 1,
    X = 2
}
";
        Assert.Equal(expected.Trim(), actual);
    }

    [Fact]
    public void T05f_Should_create_SetCreateObject()
    {
        var uu = new FluentCode()
            .SetCreateObject("EditedRow", "new X(1, 2)", false, false);

        var cs = new CsCodeWriter();
        uu.Write(cs);
        var actual = cs.Code.Trim();
        _testOutputHelper.WriteLine(actual);
        const string expected = "EditedRow = new X(1, 2)";
        Assert.Equal(expected.Trim(), actual);
    }
}