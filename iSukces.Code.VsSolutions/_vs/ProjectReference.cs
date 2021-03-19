using System.IO;
using System.Xml.Linq;

namespace iSukces.Code.VsSolutions
{
    public class ProjectReference
    {
        // Public Methods 

        public static ProjectReference FromNode(XElement reference, DirectoryInfo baseDir)
        {
            var hintPathElement = reference.Element(reference.Name.Namespace + "HintPath");
            var hintPath        = hintPathElement == null ? null : hintPathElement.Value;
            return new ProjectReference
            {
                Name = (string)reference.Attribute(Tags.Include),
                HintPath = string.IsNullOrEmpty(hintPath)
                    ? null
                    : new FileInfo(Path.Combine(baseDir.FullName, hintPath))
            };
        }

        public static ProjectReference FromNode(XElement arg)
        {
            var ns = arg.Name.Namespace;
            return new ProjectReference
            {
                Name    = arg.Element(ns + "Name")?.Value,
                Include = (string)arg.Attribute(Tags.Include)
            };
            /*
    * <ProjectReference Include="..\Effective.Interfaces\Effective.Interfaces.csproj" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
<Project>{C0E660E6-F35C-4738-AAE6-041DDD9A6EA0}</Project>
<Name>Effective.Interfaces</Name>
</ProjectReference>
    */
        }

        public override string ToString() => Name;

        public FileInfo HintPath { get; set; }

        public string Include { get; set; }

        public string Name { get; set; }
    }
}