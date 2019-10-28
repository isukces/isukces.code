using System;
using System.Xml.Linq;

namespace isukces.code.vssolutions
{
    public class PackagesConfigItem : INuspec
    {
        public static PackagesConfigItem FromXElement(XElement arg)
        {
            if (arg == null)
                throw new NullReferenceException(nameof(arg));
            return new PackagesConfigItem
            {
                Id              = (string)arg.Attribute(PackagesConfig.Id),
                PackageVersion  = NugetVersion.FromAttribute(arg.Attribute(PackagesConfig.Version)),
                TargetFramework = (string)arg.Attribute(PackagesConfig.TargetFramework)
            };
        }

        public bool MachtId(string id) => string.Equals(id?.Trim(), Id?.Trim(), StringComparison.OrdinalIgnoreCase);

        public override string ToString() => "PackageInfo: " + Id + " " + PackageVersion;

        public string TargetFramework { get; set; }

        public NugetVersion PackageVersion { get; set; }

        public string Id { get; set; }
    }
}