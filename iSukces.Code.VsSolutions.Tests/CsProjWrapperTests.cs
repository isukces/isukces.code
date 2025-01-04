using System.Collections.Generic;
using System.Xml.Linq;
using Xunit;

namespace iSukces.Code.VsSolutions.Tests;

public class CsProjWrapperTests
{
    [Fact]
    public void T01_ShouldModify()
    {
        var a = "<Project Sdk=\"Microsoft.NET.Sdk\" />";
        var x = XDocument.Parse(a);
        var w = new CsProjWrapper(x, CsprojDocumentKind.Project);
        w.Authors.Value         = "John Doe";
        w.TargetFramework.Value = "net5.0;net6.0";
        w.Company.Value         = "iSukces";
        w.Copyright.Value       = "Copyright © Internet Sukces Piotr Stęclik 2016-2025";
        
        var    doc      = x.ToString();
        string expected = """
                          <Project Sdk="Microsoft.NET.Sdk">
                            <PropertyGroup>
                              <Authors>John Doe</Authors>
                              <TargetFrameworks>net5.0;net6.0</TargetFrameworks>
                              <Company>iSukces</Company>
                              <Copyright>Copyright © Internet Sukces Piotr Stęclik 2016-2025</Copyright>
                            </PropertyGroup>
                          </Project>
                          """;
        Assert.Equal(expected, doc);
    }
}
