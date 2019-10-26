using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace isukces.code.vssolutions
{
    internal class VsOldProjectFile : VsProjectFile
    {
        public VsOldProjectFile(XDocument document) : base(document)
        {
        }

        public IEnumerable<ProjectReference> GetProjectReferences()
        {
            var tmp = ScanItemGroups("ProjectReference").ToArray();
            if (tmp.Length == 0)
                return new ProjectReference[0];
            return tmp.Select(ProjectReference.FromNode);

            return new ProjectReference[0];
        }

        private IEnumerable<XElement> ScanItemGroups(string name)
        {
            var ns = Document.Root.Name.Namespace;
            var nodes = Document.Root?
                .Elements(ns + "ItemGroup")
                .SelectMany(a => a.Elements(ns + name));
            return nodes;
        }

        private IEnumerable<XElement> ScanPropertyGroups(string name)
        {
            var ns = Document.Root.Name.Namespace;
            var nodes = Document.Root?
                .Elements(ns + "PropertyGroup")
                .SelectMany(a => a.Elements(ns + name));
            return nodes;
        }

        public string TargetFrameworkVersion
        {
            get { return ScanPropertyGroups("TargetFrameworkVersion").FirstOrDefault()?.Value; }
        }

        public string OutputType
        {
            get { return ScanPropertyGroups("OutputType").FirstOrDefault()?.Value; }
        }

        public string AssemblyOriginatorKeyFile
        {
            get
            {
                var q = ScanPropertyGroups("AssemblyOriginatorKeyFile")
                    .Select(a => a.Value.Trim())
                    .Where(a => !string.IsNullOrEmpty(a));
                return q.FirstOrDefault();
            }
        }

/*

        public void AddPackage(PackagesConfig.PackageInfo packageInfo, string originalId, FileInfo location)
        {
            /*var q = $"nuget install {packageInfo.Id} -Version {packageInfo.Version}";
            var f  = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".bat");
            var fi = new FileInfo(f);

            q = $"{location.FullName.Substring(0, 2)}\r\ncd \"{location.Directory.FullName}\"\r\n{q}";
            File.WriteAllText(f, q);
            
            var startInfo = new ProcessStartInfo(fi.FullName)
            {
                // WindowStyle            = ProcessWindowStyle.Minimized,
                WorkingDirectory       = fi.Directory.FullName,
                CreateNoWindow         = true,
                RedirectStandardOutput = true,
                RedirectStandardError  = true,
                UseShellExecute        = false,
            };
            try
            {
                var ii = Process.Start(startInfo);
                if (ii != null)
                {
                    ii.OutputDataReceived += (a, b)=>
                    {
                        Console.WriteLine(b.Data);
                    };
                    ii.ErrorDataReceived += (a, b)=>
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(b.Data);
                        Console.ResetColor();
                    };
                    ii.BeginOutputReadLine();
                    ii.BeginErrorReadLine();
                    ii.WaitForExit();
                }
 
            }
            finally
            {
                File.Delete(f);
            }#1#
            
        }
        */
    }


    /*
     
       <PropertyGroup>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
    <ProjectGuid>{44E71E75-6EBF-4049-87D6-B1CE52241724}</ProjectGuid>
    <OutputType>Library</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>Effective.Logic</RootNamespace>
    <AssemblyName>Effective.Logic</AssemblyName>
    <TargetFrameworkVersion>v4.6.1</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
  </PropertyGroup>
*/
}