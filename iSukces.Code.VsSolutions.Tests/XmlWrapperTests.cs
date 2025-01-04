using System.Xml.Linq;
using Xunit;

namespace iSukces.Code.VsSolutions.Tests;

public class XmlWrapperTests
{
    [Fact]
    public void T01_ShouldCreateName()
    {
        const string txt = """
                           <?xml version="1.0" encoding="utf-8"?>
                           <Project Sdk="Microsoft.NET.Sdk">
                             <PropertyGroup>
                               <Copyright>Copyright © Internet Sukces Piotr Stęclik 2024-2025</Copyright>
                               <Authors>Internet Sukces Piotr Stęclik</Authors>
                               <TargetFramework>net9.0</TargetFramework>
                             </PropertyGroup>
                             <ItemGroup>
                               <ProjectReference Include="..\CommonProjects\CommonProjects.csproj" />
                             </ItemGroup>
                           </Project>
                           """;
        var doc      = XDocument.Parse(txt);
        var w        = new XmlWrapper(doc);
        var name     = w.MakeName("a");
        var expected = (XName)"a";
        Assert.Equal(expected, name);
    }

    [Fact]
    public void T02_ShouldCreateName()
    {
        const string txt = """
                           <?xml version="1.0" encoding="utf-8"?>
                           <Project ToolsVersion="15.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
                             <PropertyGroup>
                               <Copyright>Copyright © Internet Sukces Piotr Stęclik 2024-2025</Copyright>
                               <Authors>Internet Sukces Piotr Stęclik</Authors>
                               <TargetFramework>net9.0</TargetFramework>
                             </PropertyGroup>
                             <ItemGroup>
                               <ProjectReference Include="..\CommonProjects\CommonProjects.csproj" />
                             </ItemGroup>
                           </Project>
                           """;
        var        doc      = XDocument.Parse(txt);
        var        w        = new XmlWrapper(doc);
        var        name     = w.MakeName("a");
        XNamespace ns       = "http://schemas.microsoft.com/developer/msbuild/2003";
        var        expected = ns + "a";
        Assert.Equal(expected, name);
    }
}
