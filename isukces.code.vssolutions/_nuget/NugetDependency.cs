using System.Xml.Linq;

namespace isukces.code.vssolutions
{
    public class NugetDependency
    {
        public static NugetDependency FromNode(XElement x)
        {
            var ver = (string)x.Attribute("version");
            return new NugetDependency
            {
                Id       = (string)x.Attribute("id"),
                Versions = string.IsNullOrEmpty(ver) ? NugetVersionRange.Any : NugetVersionRange.Parse(ver)
            };
        }

        // Public Methods 
        public override string ToString() => string.Format("NugetDependency {0} {1}", Id, Versions);

        public string Id { get; private set; }

        public NugetVersionRange Versions { get; private set; }
    }
}