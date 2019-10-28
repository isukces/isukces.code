using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace isukces.code.vssolutions
{
    public class VsCoreProjectFile : VsProjectFile
    {
        public VsCoreProjectFile(XDocument document) : base(document)
        {
        }


        public bool AddPackage(PackagesConfig.PackageInfo packageInfo, string forceSearchId = null)
        {
            var result = false;
            if (string.IsNullOrEmpty(forceSearchId))
                forceSearchId = packageInfo.Id;
            var message = "";
            foreach (var j in PackageReferences)
            {
                var ver = j.Element(j.Name.Namespace + Tags.Version);
                if (ver != null)
                {
                    j.SetAttributeValue(Tags.Version, ver.Value?.Trim());
                    ver.Remove();
                    result = true;
                }

                if (!string.Equals(forceSearchId, (string)j.Attribute(Tags.Include),
                    StringComparison.OrdinalIgnoreCase)) continue;
                if ((string)j.Attribute(Tags.Include) != packageInfo.Id)
                {
                    message += "Change " + (string)j.Attribute(Tags.Include) + " => " + packageInfo.Id;
                    j.SetAttributeValue(Tags.Include, packageInfo.Id);
                    result = true;
                }

                if ((string)j.Attribute(Tags.Version) != packageInfo.Version)
                {
                    if (string.IsNullOrEmpty(message))
                        message = packageInfo.Id;
                    message += $", change version {(string)j.Attribute(Tags.Version)}=>{packageInfo.Version}";
                    j.SetAttributeValue(Tags.Version, packageInfo.Version);
                    result = true;
                }

                Console.WriteLine(message);
                return result;
            }

            Console.WriteLine("Add " + packageInfo.Id + " " + packageInfo.Version);
            var pNode = FindOrCreateElement(Tags.ItemGroup);
            pNode.Add(new XElement(Tags.PackageReference,
                new XAttribute(Tags.Include, packageInfo.Id),
                new XAttribute(Tags.Version, packageInfo.Version)
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
    <ProjectReference Include="..\isukces.code.Serenity\isukces.code.Serenity.csproj" />
    <ProjectReference Include="..\isukces.code.vssolutions\isukces.code.vssolutions.csproj" />
    <ProjectReference Include="..\isukces.code\isukces.code.csproj" />
  </ItemGroup>
             */
            foreach (var pr in ProjectReferences)
                if (string.Equals(projectReference.Include, (string)pr.Attribute(Tags.Include),
                    StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.Equals(projectReference.Include, (string)pr.Attribute(Tags.Include),
                        StringComparison.Ordinal)) 
                        pr.SetAttributeValue(Tags.Include, projectReference.Include);

                    return;
                }

            var pNode = FindOrCreateElement(Tags.ItemGroup);
            pNode.Add(new XElement(Tags.ProjectReference,
                new XAttribute(Tags.Include, projectReference.Include)
            ));
        }


        public IEnumerable<PackagesConfig.PackageInfo> GetReferencedPackages()
        {
            foreach (var j in PackageReferences)
                yield return new PackagesConfig.PackageInfo
                {
                    Id      = (string)j.Attribute(Tags.Include),
                    Version = (string)j.Attribute(Tags.Version)
                };
        }

        public bool RemovePackage(PackagesConfig.PackageInfo packageInfo, string forceSearchId = null)
        {
            var result = false;
            if (string.IsNullOrEmpty(forceSearchId))
                forceSearchId = packageInfo.Id;
            var message = "";
            foreach (var j in PackageReferences)
            {
                if (!string.Equals(forceSearchId, (string)j.Attribute(Tags.Include),
                    StringComparison.OrdinalIgnoreCase)) continue;
                message += "Remove " + (string)j.Attribute(Tags.Include) + " => " + packageInfo.Id;
                j.Remove();
                result = true;
            }

            Console.WriteLine(message);
            return result;
        }

        public void RemovePackage(string modificationId)
        {
            foreach (var pr in PackageReferences)
            {
                if (!string.Equals(modificationId, (string)pr.Attribute(Tags.Include),
                    StringComparison.OrdinalIgnoreCase)) continue;
                pr.Remove();
                Console.WriteLine("Remove " + modificationId + " " + (string)pr.Attribute(Tags.Version));
                return;
            }
        }

        public void Save(string iFullName)
        {
            Document.Save(iFullName);
        }

        public static VsCoreProjectFile Empty
        {
            get
            {
                var empty = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Project Sdk=""Microsoft.NET.Sdk"">
</Project>";
                var doc = XDocument.Parse(empty);
                return new VsCoreProjectFile(doc);
            }
        }

        private XElement[] PackageReferences
        {
            get
            {
                return Document
                    .Root
                    .Elements(Tags.ItemGroup)
                    .SelectMany(i => i.Elements(Tags.PackageReference))
                    .ToArray();
            }
        }
        
        private XElement[] ProjectReferences
        {
            get
            {
                return Document
                    .Root
                    .Elements(Tags.ItemGroup)
                    .SelectMany(i => i.Elements(Tags.ProjectReference))
                    .ToArray();
            }
        }

        public bool IsWeb
        {
            get
            {
                var sdk = (string)Document.Root?.Attribute(Tags.Sdk);
                return sdk == Tags.MicrosoftNetSdkWeb;
            }
        }

        public string TargetFramework
        {
            get { return FromElement("PropertyGroup/TargetFramework"); }
            set { SetValue("PropertyGroup/TargetFramework", value); }
        }

        public string Version
        {
            get { return FromElement("PropertyGroup/Version"); }
            set { SetValue("PropertyGroup/Version", value); }
        }


        public string Authors
        {
            get { return FromElement("PropertyGroup/Authors"); }
            set { SetValue("PropertyGroup/Authors", value); }
        }

        public string Company
        {
            get { return FromElement("PropertyGroup/Company"); }
            set { SetValue("PropertyGroup/Company", value); }
        }

        public string Description
        {
            get { return FromElement("PropertyGroup/Description"); }
            set { SetValue("PropertyGroup/Description", value); }
        }


        public string Copyright
        {
            get { return FromElement("PropertyGroup/Copyright"); }
            set { SetValue("PropertyGroup/Copyright", value); }
        }


        public string PackageTags
        {
            get { return FromElement("PropertyGroup/PackageTags"); }
            set { SetValue("PropertyGroup/PackageTags", value); }
        }

        public string AssemblyVersion
        {
            get { return FromElement("PropertyGroup/AssemblyVersion"); }
            set { SetValue("PropertyGroup/AssemblyVersion", value); }
        }

        public string FileVersion
        {
            get { return FromElement("PropertyGroup/FileVersion"); }
            set { SetValue("PropertyGroup/FileVersion", value); }
        }

        public string PackageVersion
        {
            get { return FromElement("PropertyGroup/Version"); }
            set { SetValue("PropertyGroup/Version", value); }
        }

        public string AssemblyOriginatorKeyFile
        {
            get { return FromElement("PropertyGroup/AssemblyOriginatorKeyFile"); }
            set { SetValue("PropertyGroup/AssemblyOriginatorKeyFile", value); }
        }

        public bool SignAssembly
        {
            get
            {
                return string.Equals(FromElement("PropertyGroup/SignAssembly"), "true",
                    StringComparison.OrdinalIgnoreCase);
            }
            set { SetValue("PropertyGroup/SignAssembly", value.ToString().ToLower()); }
        }

        public bool GeneratePackageOnBuild
        {
            get
            {
                return string.Equals(FromElement("PropertyGroup/GeneratePackageOnBuild"), "true",
                    StringComparison.OrdinalIgnoreCase);
            }
            set { SetValue("PropertyGroup/GeneratePackageOnBuild", value.ToString().ToLower()); }
        }


        public bool IncludeSymbols
        {
            get
            {
                return string.Equals(FromElement("PropertyGroup/IncludeSymbols"), "true",
                    StringComparison.OrdinalIgnoreCase);
            }
            set { SetValue("PropertyGroup/IncludeSymbols", value.ToString().ToLower()); }
        }

        public bool IncludeSource
        {
            get
            {
                return string.Equals(FromElement("PropertyGroup/IncludeSource"), "true",
                    StringComparison.OrdinalIgnoreCase);
            }
            set { SetValue("PropertyGroup/IncludeSource", value.ToString().ToLower()); }
        }


        public string PackageDescription
        {
            get { return FromElement("PropertyGroup/Description"); }
            set { SetValue("PropertyGroup/Description", value); }
        }

        public string PackageId
        {
            get { return FromElement("PropertyGroup/PackageId"); }
            set { SetValue("PropertyGroup/PackageId", value); }
        }

        public string Product
        {
            get { return FromElement("PropertyGroup/Product"); }
            set { SetValue("PropertyGroup/Product", value); }
        }

        public string DocumentationFile
        {
            get { return FromElement("PropertyGroup/DocumentationFile"); }
            set { SetValue("PropertyGroup/DocumentationFile", value); }
        }
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
    <PackageReference Include=""isukces.code"" Version=""1.0.17221.22"" />
    <PackageReference Include=""newtonsoft.json"" Version=""10.0.3"" />
  </ItemGroup>
</Project>
     
     */
}