using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace iSukces.Code.VsSolutions
{
    public sealed class SolutionProject : IEquatable<SolutionProject>
    {
        public static bool operator ==(SolutionProject left, SolutionProject right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SolutionProject left, SolutionProject right)
        {
            return !Equals(left, right);
        }

        private static VsProjectKind FindProjectType(XDocument x)
        {
            var root = x?.Root;
            if (root == null) return VsProjectKind.Unknown;
            var isNew = root.Name.LocalName == "Project" && Tags.MicrosoftNetSdk == (string)root.Attribute(Tags.Sdk);
            return isNew ? VsProjectKind.Core : VsProjectKind.Legacy;
        }

        private static string FindTargetFrameworkVersion(XDocument xDocument)
        {
            string GetPropertyGroup(string name)
            {
                var root = xDocument?.Root;
                if (root == null) return null;
                var ns = root.Name.Namespace;
                foreach (var i in root.Elements(ns + "PropertyGroup"))
                foreach (var j in i.Elements(ns + name))
                {
                    var result = j.Value.Trim();
                    if (!string.IsNullOrEmpty(result))
                        return result;
                }

                return null;
            }
            var g = FindProjectType(xDocument);
            switch (g)
            {
                case VsProjectKind.Legacy: 
                    return GetPropertyGroup("TargetFrameworkVersion");
                case VsProjectKind.Core: 
                    return GetPropertyGroup("TargetFramework");
                default: return null;
            }
        }

        private static NugetPackage[] ReadNugetPackagesFromCsProj(XDocument doc)
        {
            var root = doc?.Root;
            if (root is null)
                return XArray.Empty<NugetPackage>();
            var ns = root.Name.Namespace;
            var refNodes = root.Elements(ns + Tags.ItemGroup)
                .SelectMany(q => q.Elements(ns + Tags.PackageReference))
                .ToArray();
            return refNodes.Select(q =>
            {
                var r = new NugetPackage
                {
                    Id = (string)q.Attribute(Tags.Include)
                };
                var ver = (string)q.Attribute(Tags.Version);

                if (string.IsNullOrEmpty(ver))
                {
                    ver = (string)q.Element(q.Name.Namespace+"Version")?.Value;
                }

                if (string.IsNullOrEmpty(ver))
                    ver = "0.0.0.0";
                    //throw new Exception("Unable to get version");
                r.Version = NugetVersion.Parse(ver);
                return r;
            })
                .ToArray();
        }

        public bool Equals(SolutionProject other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other)
                   || Equals(Location, other.Location);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((SolutionProject)obj);
        }
 

        public override int GetHashCode()
        {
            var tmp = Location;
            return tmp != null ? tmp.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return string.Format("Project {0}", Location);
        }

        private IEnumerable<AssemblyBinding> AssemblyBindingsInternal()
        {
            var configFileInfo = Location.GetAppConfigFile();
            var cfg            = new AppConfig(configFileInfo);

            return cfg.GetAssemblyBindings();
        }

        private List<ProjectReference> GetReferences()
        {
            // <Project Sdk="Microsoft.NET.Sdk">
            var xml    = FileHelper.Load(Location);
            var root   = xml.Root;
            var result = new List<ProjectReference>();
            if (root == null) return result;

            if (Kind == VsProjectKind.Core)
            {
                var doc = FileHelper.Load(Location);
                var refNodes = doc?.Root?.Elements(Tags.ItemGroup).SelectMany(q => q.Elements("Reference")).ToArray() ??
                               XArray.Empty<XElement>();
                return refNodes.Select(q => { return ProjectReference.FromNode(q, Location.Directory); }).ToList();
            }

            foreach (var itemGroupElement in root.Elements(root.Name.Namespace + Tags.ItemGroup))
            foreach (var reference in itemGroupElement.Elements(itemGroupElement.Name.Namespace + "Reference"))
                result.Add(ProjectReference.FromNode(reference, Location.Directory));
            return result;
        }

        private NugetPackage[] NugetPackagesInternal()
        {
            try
            {
                var doc1       = FileHelper.Load(Location);
                var fromCsProj = ReadNugetPackagesFromCsProj(doc1);
                if (Kind == VsProjectKind.Core)
                    /*<Project Sdk="Microsoft.NET.Sdk">
          <PropertyGroup>
            <OutputType>Exe</OutputType>
            <TargetFramework>net461</TargetFramework>
          </PropertyGroup>
          <ItemGroup>
            <PackageReference Include="Newtonsoft.Json" Version="10.0.3" />
          </ItemGroup>
          <ItemGroup>
            <ProjectReference Include="..\conexx.translate\conexx.translate.csproj">
              <Project>{794DB2FD-85E2-456E-8DCA-A54EE5C037B9}</Project>
              <Name>conexx.translate</Name>
            </ProjectReference>
          </ItemGroup>
          <ItemGroup>
            <Reference Include="Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed">
              <HintPath>..\packages\Newtonsoft.Json.10.0.3\lib\net45\Newtonsoft.Json.dll</HintPath>
            </Reference>
          </ItemGroup>
        </Project>*/
                    return fromCsProj;

                // ReSharper disable once PossibleNullReferenceException
                var configFileInfo = Location.GetPackagesConfigFile();
                if (!configFileInfo.Exists)
                    return fromCsProj;
                var xml  = FileHelper.Load(configFileInfo);
                var root = xml.Root;
                if (root == null || root.Name.LocalName != "packages")
                    return fromCsProj;
                var packages = root.Elements(root.Name.Namespace + "package");
                var result   = packages.Select(NugetPackage.Parse).ToArray();
                if (fromCsProj.Any()) return result.Concat(fromCsProj).ToArray();

                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to find nuget packages in project " + Location.Name + ".\r\n" + e.Message);
            }
        }


        public List<AssemblyBinding> AssemblyBindings =>
            _assemblyBindings ?? (_assemblyBindings = AssemblyBindingsInternal().ToList());

        public NugetPackage[] NugetPackages => _nugetPackages ?? (_nugetPackages = NugetPackagesInternal());

        public List<ProjectReference> References => _references ?? (_references = GetReferences());

        public FileName Location
        {
            get => _location;
            set
            {
                if (_location == value)
                    return;
                _location = value;
                var l = value.Exists ? FileHelper.Load(value) : null;
                Kind = l is null
                    ? VsProjectKind.Unknown
                    : FindProjectType(l);
                TargetFrameworkVersion = l is null
                    ? null
                    : FindTargetFrameworkVersion(l);
            }
        }

        public string TargetFrameworkVersion { get; set; }

        public VsProjectKind Kind { get; private set; }

        private List<AssemblyBinding> _assemblyBindings;
        private NugetPackage[] _nugetPackages;
        private List<ProjectReference> _references;
        private FileName _location;
    }
}