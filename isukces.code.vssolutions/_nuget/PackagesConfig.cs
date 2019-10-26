using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace isukces.code.vssolutions
{
    public class PackagesConfig : XmlWrapper
    {
        public PackagesConfig(XDocument document) : base(document)
        {
        }

        public static PackagesConfig Load(string fileName) =>
            File.Exists(fileName) ? new PackagesConfig(XDocument.Load(fileName)) : Empty;

        public IEnumerable<PackageInfo> GetPackages()
        {
            var root = Document?.Root;
            if (root == null)
                return new PackageInfo[0];
            return root.Elements("package").Select(PackageInfo.FromXElement);
        }

        public static PackagesConfig Empty
        {
            get
            {
                var doc = XDocument.Parse("<packages></packages>");
                return new PackagesConfig(doc);
            }
        }

        public class PackageInfo
        {
            public static PackageInfo FromXElement(XElement arg)
            {
                if (arg == null)
                    throw new NullReferenceException(nameof(arg));
                return new PackageInfo
                {
                    Id              = (string)arg.Attribute("id"),
                    Version         = (string)arg.Attribute("version"),
                    TargetFramework = (string)arg.Attribute("targetFramework")
                };
            }

            public bool MachtId(string id) => string.Equals(id?.Trim(), Id?.Trim(), StringComparison.OrdinalIgnoreCase);

            public override string ToString() => "PackageInfo: " + Id + " " + Version;

            public string TargetFramework { get; set; }

            public string Version { get; set; }

            public string Id { get; set; }
        }
    }
}