namespace iSukces.Code.VsSolutions.Tests;

public sealed class NumberOrTextTests
{
    [Fact]
    public void T01_Should_create()
    {
        var n = new NumberOrText("123");
        Assert.Equal("123", n.Text);
        Assert.Equal(123, n.Number);
    }
    
    [Fact]
    public void T02a_Should_compare()
    {
        NumberOrText a = 123;
        NumberOrText b = 123;
        Assert.False(a < b);
        Assert.True(a <= b);
        Assert.False(b > a);
        Assert.True(b >= a);
        Assert.True(b == a);
        Assert.False(b != a);
    }
    
    
    [Fact]
    public void T02b_Should_compare()
    {
        NumberOrText a = 123;
        NumberOrText b = 124;
        Assert.True(a < b);
        Assert.True(a <= b);
        Assert.True(b > a);
        Assert.True(b >= a);
        Assert.True(b != a);
        Assert.False(b == a);

    }
        
    
    [Fact]
    public void T02c_Should_compare()
    {
        NumberOrText a = 123;
        NumberOrText b = "124";
        Assert.True(a < b);
        Assert.True(a <= b);
        Assert.True(b > a);
        Assert.True(b >= a);
        Assert.True(b != a);
        Assert.False(b == a);
    }
    
    [Fact]
    public void T02d_Should_compare()
    {
        NumberOrText a = 123;
        NumberOrText b = "other";
        Assert.True(a < b);
        Assert.True(a <= b);
        Assert.True(b > a);
        Assert.True(b >= a);
        Assert.True(b != a);
        Assert.False(b == a);
    }    
    
    [Fact]
    public void T02e_Should_compare()
    {
        NumberOrText a = "other";
        NumberOrText b = 123;
        Assert.False(a < b);
        Assert.False(a <= b);
        Assert.True(b < a);
        Assert.True(b <= a);
        Assert.True(b != a);
        Assert.False(b == a);
    }
    
    
    
}
