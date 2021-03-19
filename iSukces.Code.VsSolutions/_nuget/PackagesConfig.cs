using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace iSukces.Code.VsSolutions
{
    public class PackagesConfig : XmlWrapper
    {
        public PackagesConfig(XDocument document) : base(document)
        {
        }

        public static PackagesConfig Load(string fileName) =>
            File.Exists(fileName) ? new PackagesConfig(XDocument.Load(fileName)) : Empty;

        public static PackagesConfig Load(DirectoryInfo dir)
        {
            var filename = Path.Combine(dir.FullName, DefaultFilename);
            return Load(filename);
        }


        public IEnumerable<PackagesConfigItem> GetPackages()
        {
            var root = Document?.Root;
            if (root == null)
                return new PackagesConfigItem[0];
            return root.Elements(Package).Select(PackagesConfigItem.FromXElement);
        }

        public static PackagesConfig Empty
        {
            get
            {
                var doc = XDocument.Parse("<packages></packages>");
                return new PackagesConfig(doc);
            }
        }

        public const string TargetFramework = "targetFramework";
        public const string Id = "id";
        public const string Version = "version";
        public const string Package = "package";
        public const string DefaultFilename = "packages.config";
    }
}