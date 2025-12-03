namespace iSukces.Code.VsSolutions.Tests;

public class NugetVersionTests
{
    [Fact]
    public void T01_Should_parse()
    {
        var q = NugetVersion.Parse("4.3.0+build.251.sha.509556a");
        Assert.NotNull(q);

    }
}