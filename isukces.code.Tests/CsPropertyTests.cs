using Xunit;

namespace iSukces.Code.Tests;

public class CsPropertyTests
{
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
                .WithOwnGetterAsExpression("GetValue()")
                .WithOwnSetterAsExpression("CallMethod(value)")
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
            get { return GetValue(); }
            set { CallMethod(value); }
        } = 1;

    }
}
";
        Assert.Equal(exp.Trim(), code.Trim());
        
    }

}