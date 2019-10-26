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
                var ver = j.Element(j.Name.Namespace + "Version");
                if (ver != null)
                {
                    j.SetAttributeValue("Version", ver.Value?.Trim());
                    ver.Remove();
                    result = true;
                }

                if (!string.Equals(forceSearchId, (string)j.Attribute("Include"),
                    StringComparison.OrdinalIgnoreCase)) continue;
                if ((string)j.Attribute("Include") != packageInfo.Id)
                {
                    message += "Change " + (string)j.Attribute("Include") + " => " + packageInfo.Id;
                    j.SetAttributeValue("Include", packageInfo.Id);
                    result = true;
                }

                if ((string)j.Attribute("Version") != packageInfo.Version)
                {
                    if (string.IsNullOrEmpty(message))
                        message = packageInfo.Id;
                    message += $", change version {(string)j.Attribute("Version")}=>{packageInfo.Version}";
                    j.SetAttributeValue("Version", packageInfo.Version);
                    result = true;
                }

                Console.WriteLine(message);
                return result;
            }

            Console.WriteLine("Add " + packageInfo.Id + " " + packageInfo.Version);
            var pNode = FindOrCreateElement("ItemGroup");
            pNode.Add(new XElement("PackageReference",
                new XAttribute("Include", packageInfo.Id),
                new XAttribute("Version", packageInfo.Version)
            ));
            return true;
        }


        public IEnumerable<PackagesConfig.PackageInfo> GetReferencedPackages()
        {
            foreach (var j in PackageReferences)
                yield return new PackagesConfig.PackageInfo
                {
                    Id      = (string)j.Attribute("Include"),
                    Version = (string)j.Attribute("Version")
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
                if (!string.Equals(forceSearchId, (string)j.Attribute("Include"),
                    StringComparison.OrdinalIgnoreCase)) continue;
                message += "Remove " + (string)j.Attribute("Include") + " => " + packageInfo.Id;
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
                if (!string.Equals(modificationId, (string)pr.Attribute("Include"),
                    StringComparison.OrdinalIgnoreCase)) continue;
                pr.Remove();
                Console.WriteLine("Remove " + modificationId + " " + (string)pr.Attribute("Version"));
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
                    .Elements("ItemGroup")
                    .SelectMany(i => i.Elements("PackageReference"))
                    .ToArray();
            }
        }

        public bool IsWeb
        {
            get
            {
                var sdk = (string)Document.Root?.Attribute("Sdk");
                return sdk == "Microsoft.NET.Sdk.Web";
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