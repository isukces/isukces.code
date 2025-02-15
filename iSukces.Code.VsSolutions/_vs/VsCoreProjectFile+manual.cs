using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace iSukces.Code.VsSolutions;

public partial class VsCoreProjectFile
{
    public bool AddPackage(INuspec request, string? forceSearchId = null)
    {
        var result = false;
        if (string.IsNullOrEmpty(forceSearchId))
            forceSearchId = request.Id;
        // var message = "";
        foreach (var xElement in PackageReferences)
        {
            var ver = xElement.Element(xElement.Name.Namespace + Tags.Version);
            if (ver != null)
            {
                // change node "Version" info attribute
                xElement.SetAttributeValue(Tags.Version, ver.Value?.Trim());
                ver.Remove();
                result = true;
            }

            if (!string.Equals(forceSearchId, (string?)xElement.Attribute(Tags.Include!),
                    StringComparison.OrdinalIgnoreCase)) continue;
            if ((string?)xElement.Attribute(Tags.Include!) != request.Id)
            {
                //message += "Change " + (string?)xElement.Attribute(Tags.Include!) + " => " + packageInfo.Id;
                xElement.SetAttributeValue(Tags.Include!, request.Id);
                result = true;
            }

            var jVersion = NugetVersion.FromAttribute(xElement.Attribute(Tags.Version));
            if (jVersion != request.PackageVersion)
            {
                /*if (string.IsNullOrEmpty(message))
                    message = packageInfo.Id;
                message += $", change version {(string?)xElement.Attribute(Tags.Version)}=>{packageInfo.PackageVersion}";*/
                xElement.SetAttributeValue(Tags.Version, request.PackageVersion);
                result = true;
            }

            //Console.WriteLine(message);
            return result;
        }

        //Console.WriteLine("Add " + request.Id + " " + request.PackageVersion);
        var pNode = FindOrCreateElement(Tags.ItemGroup);
        pNode.Add(new XElement(Tags.PackageReference,
            new XAttribute(Tags.Include!, request.Id),
            new XAttribute(Tags.Version, request.PackageVersion)
        ));
        return true;
    }


    public void AddProjectReference(ProjectReference projectReference)
    {
        /*
<ItemGroup>
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="16.2.0" />
<PackageReference Include="xunit" Version="2.4.1" />
<PackageReference Include="xunit.abstractions" Version="2.0.3" />
<PackageReference Include="xunit.assert" Version="2.4.1" />
<PackageReference Include="xunit.core" Version="2.4.1" />
<PackageReference Include="xunit.extensibility.core" Version="2.4.1" />
<PackageReference Include="xunit.extensibility.execution" Version="2.4.1" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.4.1" />
<ProjectReference Include="..\iSukces.Code.Serenity\iSukces.Code.Serenity.csproj" />
<ProjectReference Include="..\iSukces.Code.VsSolutions\iSukces.Code.VsSolutions.csproj" />
<ProjectReference Include="..\iSukces.Code\iSukces.Code.csproj" />
</ItemGroup>
         */
        foreach (var pr in ProjectReferences)
            if (string.Equals(projectReference.Include, (string?)pr.Attribute(Tags.Include!),
                    StringComparison.OrdinalIgnoreCase))
            {
                if (!string.Equals(projectReference.Include, (string?)pr.Attribute(Tags.Include!),
                        StringComparison.Ordinal))
                    pr.SetAttributeValue(Tags.Include!, projectReference.Include);

                return;
            }

        var pNode = FindOrCreateElement(Tags.ItemGroup);
        pNode.Add(new XElement(Tags.ProjectReference!,
            new XAttribute(Tags.Include!, projectReference.Include)
        ));
    }


    public IEnumerable<PackagesConfigItem> GetReferencedPackages()
    {
        foreach (var j in PackageReferences)
            yield return new PackagesConfigItem
            {
                Id             = (string?)j.Attribute(Tags.Include!),
                PackageVersion = NugetVersion.FromAttribute(j.Attribute(Tags.Version!))
            };
    }

    public bool RemovePackage(PackagesConfigItem packageInfo, string? forceSearchId = null)
    {
        var result = false;
        if (string.IsNullOrEmpty(forceSearchId))
            forceSearchId = packageInfo.Id;
        //var message = "";
        foreach (var j in PackageReferences)
        {
            if (!string.Equals(forceSearchId, (string?)j.Attribute(Tags.Include!),
                    StringComparison.OrdinalIgnoreCase)) continue;
            // message += "Remove " + (string)j.Attribute(Tags.Include!) + " => " + packageInfo.Id;
            j.Remove();
            result = true;
        }

        // Console.WriteLine(message);
        return result;
    }

    public bool RemovePackage(string packageId)
    {
        foreach (var pr in PackageReferences)
        {
            if (!string.Equals(packageId, (string?)pr.Attribute(Tags.Include!),
                    StringComparison.OrdinalIgnoreCase)) continue;
            pr.Remove();
            // Console.WriteLine("Remove " + packageId + " " + (string?)pr.Attribute(Tags.Version));
            return true;
        }

        return false;
    }

    public void Save(string iFullName)
    {
        Document.Save(iFullName);
    }

    #region Properties

    public static VsCoreProjectFile Empty
    {
        get
        {
            var empty = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Project Sdk=""Microsoft.NET.Sdk"">
</Project>";
            var doc = XDocument.Parse(empty);
            return new VsCoreProjectFile(doc, CsprojDocumentKind.Project);
        }
    }

    private XElement[] PackageReferences
    {
        get
        {
            return Document.Root!
                .Elements(Tags.ItemGroup)
                .SelectMany(i => i.Elements(Tags.PackageReference))
                .ToArray();
        }
    }

    private XElement[] ProjectReferences
    {
        get
        {
            return Document.Root!
                .Elements(Tags.ItemGroup)
                .SelectMany(i => i.Elements(Tags.ProjectReference))
                .ToArray();
        }
    }

    public bool IsWeb
    {
        get
        {
            var sdk = (string?)Document.Root?.Attribute(Tags.Sdk!);
            return sdk == Tags.MicrosoftNetSdkWeb;
        }
    }

    #endregion
}

/*
 <?xml version=""1.0"" encoding=""utf-8""?>
<Project Sdk=""Microsoft.NET.Sdk"">
<PropertyGroup>
<OutputType>Exe</OutputType>
<TargetFramework>net461</TargetFramework>
<Version>1.0.1.1</Version>
<Authors>Internet Sukces Piotr Stęclik</Authors>
<Company>Internet Sukces Piotr Stęclik</Company>
<Description>Serenity utils</Description>
<Copyright>Copyright by Internet Sukces Piotr Stęclik 2017</Copyright>
<PackageTags>Serenity, MVC</PackageTags>
<AssemblyVersion>1.0.234.24</AssemblyVersion>
<FileVersion>1.0.234.24</FileVersion>
</PropertyGroup>
<ItemGroup>
<None Remove=""isukces.Serenity.csproj.DotSettings"" />
</ItemGroup>
<ItemGroup>
<PackageReference Include=""iSukces.Code"" Version=""1.0.17221.22"" />
<PackageReference Include=""newtonsoft.json"" Version=""10.0.3"" />
</ItemGroup>
</Project>

 */

