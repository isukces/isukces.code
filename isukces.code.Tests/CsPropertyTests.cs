using iSukces.Code.Interfaces;
using Xunit;
using Xunit.Abstractions;

namespace iSukces.Code.Tests;

public class CsPropertyTests
{
    public CsPropertyTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void T01_Should_create_autoproperty()
    {
        var code = CsMethodTest.TestCode(cs =>
        {
            cs.AddProperty<int>("Prop")
                .WithNoEmitField();
        });

        const string exp = @"
// ReSharper disable All
// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace My123
{
    public class MyClass
    {
        public int Prop { get; set; }

    }
}

";
        Assert.Equal(exp.Trim(), code.Trim());
    }

    [Fact]
    public void T02_Should_create_autoproperty_with_value()
    {
        var code = CsMethodTest.TestCode(cs =>
        {
            cs.AddProperty<int>("Prop")
                .WithNoEmitField()
                .WithConstValue("1");
        });

        const string exp = @"
// ReSharper disable All
// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace My123
{
    public class MyClass
    {
        public int Prop { get; set; } = 1;

    }
}

";
        Assert.Equal(exp.Trim(), code.Trim());
    }

    [Fact]
    public void T03_Should_create_property_with_value()
    {
        var code = CsMethodTest.TestCode(cs =>
        {
            cs.AddProperty<int>("Prop")
                .WithOwnGetterAsExpressionBody("GetValue()")
                .WithOwnSetterAsExpressionBody("CallMethod(value)")
                .WithNoEmitField()
                .WithConstValue("1");
        });

        const string exp = @"
// ReSharper disable All
// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace My123
{
    public class MyClass
    {
        public int Prop
        {
            get => GetValue();
            set => CallMethod(value);
        } = 1;

    }
}
";
        Assert.Equal(exp.Trim(), code.Trim());
    }


    [Fact]
    public void T03a_Should_create_property_with_preprocess()
    {
        var code = CsMethodTest.TestCode(cs =>
        {
            cs.AddProperty<int>("Prop")
                .WithOwnGetterAsExpressionBody("GetValue()")
                .WithOwnSetterFromValue("Preprocess(value)")
                .WithNoEmitField()
                .WithConstValue("1");
        });

        const string exp = @"
// ReSharper disable All
// suggestion: File scope namespace is possible, use [AssumeDefinedNamespace]
namespace My123
{
    public class MyClass
    {
        public int Prop
        {
            get => GetValue();
            set => field = Preprocess(value);
        } = 1;

    }
}
";
        Assert.Equal(exp.Trim(), code.Trim());
    }


    [Fact]
    public void T04_Should_Create_auto_property_with_initialisation()
    {
        var cl = new CsClass((CsType)"Src1");
        var p  = cl.AddProperty("A", CsType.Int32);
        p.MakeAutoImplementIfPossible = true;
        //p.ConstValue = "12";
        // odwrotny

        var w = new CsCodeWriter();
        cl.MakeCode(w);
        var expected = @"public class Src1
{
    public int A { get; set; }

}
";
        Assert.Equal(expected.Trim(), w.GetCodeTrim());

        p.ConstValue = "12";
        w            = new CsCodeWriter();
        cl.MakeCode(w);
        expected = @"public class Src1
{
    public int A { get; set; } = 12;

}
";
        Assert.Equal(expected.Trim(), w.GetCodeTrim());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void T05_Should_Create_auto_property_with_back_field(int testNumber)
    {
        var w  = new CsCodeWriter();
        var cl = new CsClass((CsType)("TestClass" + testNumber))
        {
            Formatting = new CodeFormatting(CodeFormattingFeatures.Cs14, 120)
        };
        var p = cl.AddProperty("A", CsType.StringNullable)
            .WithMakeAutoImplementIfPossible()
            .WithBackingField();

        switch (testNumber)
        {
            case 1:
            {

                p.WithOwnSetterFromValue("value?.Trim() ?? string.Empty");

                cl.MakeCode(w, new CodeEmitConfig
                {
                    AllowReferenceNullable = true,
                    AllowPropertyBackField = true
                });
                const string expected = """
                                        public class TestClass1
                                        {
                                            public string A
                                            {
                                                get;
                                                set => field = value?.Trim() ?? string.Empty;
                                            }

                                        }
                                        """;
                Finish(expected);
                break;
            }
            case 2:
            {
                {
                    p.WithOwnSetterFromValue("value?.Trim() ?? string.Empty");
                    p.ConstValue = "12".CsEncode();
                    cl.MakeCode(w);
                    const string expected = """
                                            public class TestClass2
                                            {
                                                public string A
                                                {
                                                    get;
                                                    set => field = value?.Trim() ?? string.Empty;
                                                } = "12";

                                            }
                                            """;
                    Finish(expected);
                }
                break;
            }
            case 3:
            {
                {
                    p.WithOwnGetterAsExpressionBody("field?.Trim() ?? string.Empty");
                    p.ConstValue = "12".CsEncode();
                    cl.MakeCode(w);
                    const string expected = """
                                            public class TestClass3
                                            {
                                                public string A
                                                {
                                                    get => field?.Trim() ?? string.Empty;
                                                    set;
                                                } = "12";
                                            
                                            }
                                            """;
                    Finish(expected);
                }
                break;
            }
            case 4:
            {
                {
                    var getterCode = """
                                     var tmp = field?.Trim() ?? string.Empty;
                                     if (tmp.Length > 100)
                                         return tmp.Substring(0, 97) + "...";
                                     return tmp;
                                     """;
                    p.OwnGetter  = (PropertyGetterCode?)getterCode;
                    p.ConstValue = "12".CsEncode();
                    cl.MakeCode(w);
                    const string expected = """
                                            public class TestClass4
                                            {
                                                public string A
                                                {
                                                    get
                                                    {
                                                        var tmp = field?.Trim() ?? string.Empty;
                                                        if (tmp.Length > 100)
                                                            return tmp.Substring(0, 97) + "...";
                                                        return tmp;
                                                    }
                                                    set;
                                                } = "12";
                                            
                                            }
                                            """;
                    Finish(expected);
                }
                break;
            }
        }

        return;

        void Finish(string expected)
        {
            _testOutputHelper.WriteLine("======");
            var actual = w.GetCodeTrim();
            _testOutputHelper.WriteLine(actual);
            _testOutputHelper.WriteLine("======");
            Assert.Equal(expected.Trim(), actual);
        }
    }

    private readonly ITestOutputHelper _testOutputHelper;
    
    public class TestClass3
    {
        public string A
        {
            get => field?.Trim() ?? string.Empty;
            set;
        } = "11";
    }
}
