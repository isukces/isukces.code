using System.Xml.Linq;

namespace isukces.code.vssolutions
{
    public class NugetPackage
    {
        // Public Methods 

        public static NugetPackage Parse(XElement packageXElement)
        {
            var ver    = (string)packageXElement.Attribute(Tags.Version); 
            if (string.IsNullOrEmpty(ver))
                ver = (string)packageXElement.Attribute(Tags.VersionLower);
            var parsed = NugetVersion.Parse(ver);
            // if (!Version.TryParse(ver, out parsed))
            //   throw new Exception("Invalid version " + ver);
            return new NugetPackage
            {
                Id              = (string)packageXElement.Attribute("id"),
                Version         = parsed,
                TargetFramework = (string)packageXElement.Attribute("targetFramework")
            };
        }

        // Public Methods 

        public override string ToString() => string.Format("{0} {1} {2}", Id, Version, TargetFramework);

        public string TargetFramework { get; set; }

        public NugetVersion Version { get; set; }

        public string Id { get; set; }
    }
}